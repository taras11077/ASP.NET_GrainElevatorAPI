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

builder.Services.AddTransient<IInputInvoice, InputInvoice>();
builder.Services.AddTransient<ILaboratoryCard, LaboratoryCard>();
builder.Services.AddTransient<IRegister, Register>();
builder.Services.AddTransient<IProductionButchCalculator, StandardProductionButchCalculator>();

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

// // Ініціалізація адміністратора
// await EnsureAdminCreated(app.Services);

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



// public static async Task EnsureAdminCreated(IServiceProvider serviceProvider)
// {
//     using var scope = serviceProvider.CreateScope();
//     var context = scope.ServiceProvider.GetRequiredService<GrainElevatorApiContext>();
//
//     // Перевіряємо, чи є користувач з роллю "Admin"
//     if (!context.Employees.Any(e => e.Role == "Admin"))
//     {
//         var admin = new Employee
//         {
//             Name = "Admin",
//             Email = "admin@example.com",
//             PasswordHash = PasswordHasher.HashPassword("Admin@123"),
//             CreatedById = null,
//             CreatedAt = DateTime.UtcNow,
//             Role = "Admin"
//         };
//
//         context.Employees.Add(admin);
//         await context.SaveChangesAsync();
//     }
// }