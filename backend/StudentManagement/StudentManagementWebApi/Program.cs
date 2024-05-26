using System.Security.Claims;
using System.Text;
using MessageClient;
using MessagePublisher.Configuration;
using MessagePublisher.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotificationService.Hubs;
using StudentManagementWebApi.Attributes;
using StudentManagementWebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure services and access the connection string
builder.Configuration.AddJsonFile("appsettings.json");


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
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
        
    });

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;

});

builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfiguration"));
builder.Services.AddSingleton<RabbitMqConnectionService>();
builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddSingleton<NotificationHub>();
builder.Services.AddTransient<MessageConsumer>(provided =>
{
    var rabbitMqConnectionService = provided.GetRequiredService<RabbitMqConnectionService>();
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    var hubUrl = configuration.GetValue<string>("SignalRUrl");
    return new MessageConsumer(rabbitMqConnectionService, hubUrl);
});

//for the purpose of this project, we are disabling the implicit required attribute for non-nullable reference types
builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IStudentGroupsRepository, StudentGroupsRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IStudentGroupAssignmentRepository, StudentGroupAssignmentRepository>();
builder.Services.AddScoped<ISubjectTypesRepository, SubjectTypesRepository>();
builder.Services.AddScoped<ISubjectGroupAssignmentRepository, SubjectGroupAssignmentRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/notificationHub");
app.MapControllers();

app.Run();