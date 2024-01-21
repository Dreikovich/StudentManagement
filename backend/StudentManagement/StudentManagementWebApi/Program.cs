
using Microsoft.OpenApi.Models;
using StudentManagementWebApi.Attributes;
using StudentManagementWebApi.Repositories;

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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", build =>
    {
        build.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IStudentGroupsRepository, StudentGroupsRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IStudentGroupAssignmentRepository, StudentGroupAssignmentRepository>();
builder.Services.AddScoped<ISubjectTypesRepository, SubjectTypesRepository>();
builder.Services.AddScoped<ISubjectGroupAssignmentRepository, SubjectGroupAssignmentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAll");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();