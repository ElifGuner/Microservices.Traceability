using MassTransit;
using MassTransit.Transports;
using Shared;
using System.Diagnostics;
using System.Text.Json;

namespace Publisher.Services
{
    public class PublishMessageService : BackgroundService
    {
        IPublishEndpoint _publishEndpoint;
        ILogger<PublishMessageService> _logger;
        public PublishMessageService(IPublishEndpoint publishEndpoint, ILogger<PublishMessageService> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var correlationId = Guid.NewGuid();

            int i = 0;
            while (true)
            {
                ExampleMessage message = new()
                {
                    Text = $"{++i}. mesaj"
                };

                Trace.CorrelationManager.ActivityId = correlationId;
                _logger.LogDebug("Publisher log");

                await Console.Out.WriteLineAsync($"{JsonSerializer.Serialize(message)} - Correlation Id : {correlationId}");
                await _publishEndpoint.Publish(message, async context =>
                {
                    context.Headers.Set("CorrelationId", correlationId);
                });
                await Task.Delay(750);
            }
        }
    }

}