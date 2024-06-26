using System.Security.Claims;
using System.Text;
using MessageClient;
using MessagePublisher.Configuration;
using MessagePublisher.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotificationService.Hubs;
using NotificationService.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromMinutes(5);
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(5);
    options.HandshakeTimeout = TimeSpan.FromMinutes(5);
});

// Add JWT Authentication
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

        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/NotificationHub"))
                    // Read the token out of the query string
                    context.Token = accessToken;
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

                // Log all claims for debugging
                foreach (var claim in context.Principal.Claims)
                    logger.LogInformation($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");

                var userId = context.Principal
                    .FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var identity = context.Principal.Identities.FirstOrDefault(id => id.IsAuthenticated);
                    if (identity != null) identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
                }

                logger.LogInformation($"User ID from token: {userId}");

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddScoped<NotificationHub>();
builder.Services.AddTransient<RabbitMqConnectionService>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddSingleton<MessageConsumer>(provider =>
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    var rabbitMqConfiguration = configuration.GetSection("RabbitMqConfiguration").Get<RabbitMqConfiguration>();
    var rabbitMqConnectionService = new RabbitMqConnectionService(Options.Create(rabbitMqConfiguration));

    var hubUrl = configuration.GetValue<string>("SignalRUrl");
    var hubContext = provider.GetRequiredService<IHubContext<NotificationHub>>();
    var logger = provider.GetRequiredService<ILogger<MessageConsumer>>();
    var cache = provider.GetRequiredService<IDistributedCache>();
    return new MessageConsumer(rabbitMqConnectionService, hubUrl, hubContext, logger, cache);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "SampleInstance";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials());
}

app.UseRouting();
app.UseHttpsRedirection();

// Add Authentication and Authorization middleware
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");

app.Run();