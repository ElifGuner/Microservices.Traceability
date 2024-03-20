using Consumer.Consumers;
using MassTransit;
using NLog.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<ExampleMessageConsumer>();

    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host("amqps://xqnpqzag:cac8-cQJ2eaSQ7vZ91r4i18hM-h27keq@fish.rmq.cloudamqp.com/xqnpqzag");

        _configurator.ReceiveEndpoint("example-message-queue", e => e.ConfigureConsumer<ExampleMessageConsumer>(context));
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddNLog();

var host = builder.Build();
host.Run();