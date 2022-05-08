using EmpAPI.Helpers;
using EmpAPI.Models;
using EmpAPI.Repository;
using EmpAPI.Services;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = Directory.GetCurrentDirectory(),
    WebRootPath = "wwwroot"
});

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

// Add services to the container.
//builder.Services.AddCors(p => p.AddPolicy("AllowOrigin", builder =>
//{
//    builder.WithOrigins(new[] {"http://localhost:3000", "http://localhost:8000", "http://localhost:42000"}).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
//}));
builder.Services.AddCors();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
        .Json.ReferenceLoopHandling.Ignore)
        .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
        = new DefaultContractResolver());


builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddControllers();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITaxConfigRepository, TaxConfigRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseCors(options => options
    .WithOrigins(new[] {"http://localhost:3000", "http://localhost:8080", "http://localhost:4200"})
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    );
//app.UseCors("AllowOrigin");
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
    RequestPath="/Photos"
});

app.Run();
