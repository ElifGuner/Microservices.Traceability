using Microsoft.Extensions.Primitives;

namespace AspNetCoreApplication.Middlewares
{
    public class CorrelationIdMiddleware
    {
        public RequestDelegate next { get; set; }
        public CorrelationIdMiddleware(RequestDelegate _next)
        {
            next = _next;
        }

        const string correlationIdHeaderKey = "X-Correlation-ID";
        public async Task Invoke(HttpContext context, ILogger<CorrelationIdMiddleware> logger)
        {
            string correlationId = Guid.NewGuid().ToString();

            if (context.Request.Headers.TryGetValue(correlationIdHeaderKey, out StringValues _correlationId))
                correlationId = _correlationId;
            else
                context.Request.Headers.Add(correlationIdHeaderKey, correlationId); //header içinde correlationId yoksa oluşturduğumuz değeri set ediyoruz.

            NLog.MappedDiagnosticsContext.Set("CorrelationId", correlationId); //NLog'daki MDC correlationId parametresini set ediyoruz Set fonk üzerinden

            logger.LogDebug("Asp.NET Core App. CorrelationID Log Example");

            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.TryGetValue(correlationIdHeaderKey, out _)) //Response'da var mı? yoksa set et.
                    context.Response.Headers.Add(correlationIdHeaderKey, correlationId);
                return Task.CompletedTask;
            });

            context.Items["CorrelationId"] = correlationId;
            await next(context);
        }
    }
}
