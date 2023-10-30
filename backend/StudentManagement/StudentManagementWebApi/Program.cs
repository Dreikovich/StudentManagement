
using Microsoft.OpenApi.Models;
using StudentManagementWebApi.Attributes;

var builder = WebApplication.CreateBuilder(args);

// Configure services and access the connection string
builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student Management", Version = "v1" });
    // Add a custom SchemaFilter to exclude properties marked with SwaggerExcludeAttribute
    c.SchemaFilter<SwaggerExcludeFilter>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();