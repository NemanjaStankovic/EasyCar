using EasyCar.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
//options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
//    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
//    = new DefaultContractResolver());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", builder =>
    {
        builder.WithOrigins(new string[]
        {
                        "http://localhost:8080/",
                        "https://localhost:8080/",
                        "http://127.0.0.1:8080/",
                        "https://127.0.0.1:8080/",
                        "http://127.0.0.1:5500/",
                        "http://localhost:5500/",
                        "https://127.0.0.1:5500/",
                        "https://localhost:5500/",
                        "http://127.0.0.1:5501/",
                        "http://localhost:5501/",
                        "https://127.0.0.1:5501/",
                        "https://localhost:5501/",
                        "https://localhost:7276/",
                        "http://127.0.0.1:7276/",
                        "http://localhost:7276/",
                        "https://127.0.0.1:7276/"
        })
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Photos")), RequestPath = "/Photos"
});

app.UseHttpsRedirection();

app.UseCors("CORS");

app.UseAuthorization();

app.MapControllers();

app.Run();

