using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GrainElevator.Storage;
using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Calculators.Impl;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Core.Security;
using GrainElevatorAPI.Core.Services;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Local");

builder.Services.AddDbContext<GrainElevatorApiContext>(opt =>
    opt.UseSqlServer(connectionString)
        .UseLazyLoadingProxies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true })
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<ISupplierService, SupplierService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IInputInvoiceService, InputInvoiceService>();
builder.Services.AddTransient<ILaboratoryCardService, LaboratoryCardService>();
builder.Services.AddTransient<IInvoiceRegisterService, InvoiceRegisterService>();


builder.Services.AddTransient<IInputInvoice, InputInvoice>();
builder.Services.AddTransient<ILaboratoryCard, LaboratoryCard>();
builder.Services.AddTransient<IRegister, InvoiceRegister>();
builder.Services.AddTransient<IProductionBatchCalculator, StandardProductionBatchCalculator>();

builder.Services.AddControllers();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("SessionTimeout"));
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
        logger.LogError(ex, "An error occurred while creating the admin user.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession(); 

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();



async Task EnsureAdminCreated(IServiceProvider services)
{
    var configuration = services.GetRequiredService<IConfiguration>();
    var roleService = services.GetRequiredService<IRoleService>();
    var authService = services.GetRequiredService<IAuthService>();

    // отримання даних адміністратора з конфігурації
    var adminEmail = configuration["AdminSettings:AdminEmail"];
    var adminPassword = configuration["AdminSettings:AdminPassword"];

    // перевірка, чи існує роль Admin
    var adminRole = await roleService.GetRoleByTitleAsync("Admin");
    if (adminRole == null)
    {
        adminRole = new Role
        {
            Title = "Admin",
            CreatedAt = DateTime.UtcNow
        };
        await roleService.CreateRoleAsync(adminRole);
    }
    
    // перевірка, чи існує адміністратор з таким email
    var existingAdmin = await authService.FindByEmailAsync(adminEmail);
    if (existingAdmin != null)
    {
        Console.WriteLine($"Адміністратор з email {adminEmail} вже існує. Продовження виконання програми.");
        return;
    }

    // реєстрація адміністратора 
    try
    {
        var adminEmployee = await authService.Register(adminEmail, adminPassword, adminRole.Id);
        Console.WriteLine($"Адміністратор {adminEmployee.Email} успішно створений.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Сталася помилка при створенні адміністратора: {ex.Message}");
    }
}