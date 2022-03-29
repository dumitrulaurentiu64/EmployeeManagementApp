using EmpAPI.Helpers;
using EmpAPI.Repository;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
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
