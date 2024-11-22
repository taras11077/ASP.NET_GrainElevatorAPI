using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GrainElevator.Storage;
using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Core.Security;
using GrainElevatorAPI.Core.Services;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Local");

builder.Services.AddDbContext<GrainElevatorApiContext>(opt =>
    opt.UseSqlServer(connectionString)
        .UseLazyLoadingProxies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<ISupplierService, SupplierService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IInputInvoiceService, InputInvoiceService>();
builder.Services.AddTransient<ILaboratoryCardService, LaboratoryCardService>();
builder.Services.AddTransient<IInvoiceRegisterService, InvoiceRegisterService>();
builder.Services.AddTransient<IWarehouseProductCategoryService, WarehouseProductCategoryService>();
builder.Services.AddTransient<IWarehouseUnitService, WarehouseUnitService>();
builder.Services.AddTransient<IOutputInvoiceService, OutputInvoiceService>();
builder.Services.AddTransient<IPriceListItemService, PriceListItemService>();
builder.Services.AddTransient<IPriceListService, PriceListService>();
builder.Services.AddTransient<ICompletionReportOperationService, CompletionReportOperationService>();
builder.Services.AddTransient<ICompletionReportService, CompletionReportService>();
builder.Services.AddTransient<ITechnologicalOperationService, TechnologicalOperationService>();


builder.Services.AddTransient<IInputInvoice, InputInvoice>();
builder.Services.AddTransient<ILaboratoryCard, LaboratoryCard>();
builder.Services.AddTransient<IProductionBatch, ProductionBatch>();
builder.Services.AddTransient<IInvoiceRegister, InvoiceRegister>();
builder.Services.AddTransient<IRegisterCalculator, StandardRegisterCalculator>();
builder.Services.AddTransient<IProduct, Product>();
builder.Services.AddTransient<ISupplier, Supplier>();
builder.Services.AddTransient<IRole, Role>();
builder.Services.AddTransient<IWarehouseUnit, WarehouseUnit>();
builder.Services.AddTransient<IWarehouseProductCategory, WarehouseProductCategory>();
builder.Services.AddTransient<IOutputInvoice, OutputInvoice>();
builder.Services.AddTransient<ITechnologicalOperation, TechnologicalOperation>();

builder.Services.AddControllers();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".GrainElevator.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("SessionTimeout"));
    options.Cookie.HttpOnly = true; // Заборонити доступ до cookie з JS
    options.Cookie.SameSite = SameSiteMode.None; // Дозволити міждоменні запити
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Вимагати HTTPS
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // URL фронтенда
            .AllowCredentials() // дозволити cookie
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("TokenKey")!)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
        restrictedToMinimumLevel: LogEventLevel.Error)  // Записує тільки Error і вище
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

// створення адміністратора під час запуску програми
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await EnsureAdminCreated(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Сталася помилка при створенні адміністратора.");
    }
}



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession(); 
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();



async Task EnsureAdminCreated(IServiceProvider services)
{
    var configuration = services.GetRequiredService<IConfiguration>();
    var roleService = services.GetRequiredService<IRoleService>();
    var authService = services.GetRequiredService<IAuthService>();
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    var cancellationToken = CancellationToken.None;

    // отримання даних адміністратора з конфігурації
    var adminEmail = configuration["AdminSettings:AdminEmail"];
    var adminPassword = configuration["AdminSettings:AdminPassword"];

    // перевірка, чи існує роль Admin
    var adminRole = await roleService.GetRoleByTitleAsync("Admin", cancellationToken);
    if (adminRole == null)
    {
        adminRole = new Role
        {
            Title = "Admin",
            CreatedAt = DateTime.UtcNow
        };
        await roleService.CreateRoleAsync(adminRole, cancellationToken);
    }
    
    // перевірка, чи існує адміністратор з таким email
    var existingAdmin = await authService.FindByEmailAsync(adminEmail, cancellationToken);
    if (existingAdmin != null)
    {
        logger.LogInformation($"Адміністратор з email {adminEmail} вже існує. Продовження виконання програми.");
        return;
    }

    // реєстрація адміністратора 
    try
    {
        var adminEmployee = await authService.Register(adminEmail, adminPassword, adminRole.Id, cancellationToken);
        logger.LogInformation($"Адміністратор {adminEmployee.Email} успішно створений.");
    }
    catch (Exception ex)
    {
        logger.LogError($"Сталася помилка при створенні адміністратора: {ex.Message}");
    }
}