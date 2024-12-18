using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Core.DTO;
using DATN_Infrastructure;
using DATN_Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.InfrastructureConfiguration(builder.Configuration);
builder.Services.AddSingleton<IConnectionMultiplexer>(i =>
{
    var configure = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
    return ConnectionMultiplexer.Connect(configure);
});
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine
    (Directory.GetCurrentDirectory(), "wwwroot")));
builder.Services.Configure<EmailDTO>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmail, EmailReponsitory>();
builder.Services.AddScoped<QrCoder>();
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://localhost:5173") // Allow requests from localhost:3000
               .AllowAnyHeader()                   // Allow any header
               .AllowAnyMethod()                   // Allow any HTTP method
               .AllowCredentials();                // Allow credentials (if needed)
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
