using MassTransit;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Consumers
{
    public class ExampleMessageConsumer : IConsumer<ExampleMessage>
    {
        ILogger<ExampleMessageConsumer> _logger;
        public ExampleMessageConsumer(ILogger<ExampleMessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ExampleMessage> context)
        {
            var correlationId = Guid.NewGuid();
            if (context.Headers.TryGetHeader("CorrelationId", out var _correlationId))
                correlationId = Guid.Parse(_correlationId.ToString());

            Trace.CorrelationManager.ActivityId = correlationId;
            _logger.LogDebug("Consumer log");

            Console.WriteLine($"Gelen mesaj : {context.Message.Text} - Correlation Id : {(correlationId)}");
            return Task.CompletedTask;
        }
    }
}
