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