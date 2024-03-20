using MassTransit;
using NLog.Extensions.Logging;
using Publisher.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host("amqps://xqnpqzag:cac8-cQJ2eaSQ7vZ91r4i18hM-h27keq@fish.rmq.cloudamqp.com/xqnpqzag");
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddNLog();

//PublishMessageService servisini hosted ediyoruz/aya�a kald�r�yoruz.
// Constructor parametrelerine de�erlerini g�nderiyoruz.
//Sisteme constructor�ndaki parametrelere ilgili de�erleri nereden bulabilece�ini bildiriyoruz.
builder.Services.AddHostedService<PublishMessageService>(provider =>
{
    using IServiceScope scope = provider.CreateScope();
    IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetService<IPublishEndpoint>();
    var logger = scope.ServiceProvider.GetService<ILogger<PublishMessageService>>();
    return new(publishEndpoint, logger);
});

var host = builder.Build();
host.Run();