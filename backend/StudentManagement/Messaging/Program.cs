
using Messaging;
using Messaging.Configuration;
using Messaging.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfiguration"));
builder.Services.AddSingleton<RabbitMqConnectionService>();
builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();