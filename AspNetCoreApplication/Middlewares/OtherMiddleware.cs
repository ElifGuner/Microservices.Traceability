namespace AspNetCoreApplication.Middlewares
{
    public class OtherMiddleware
    {
        public RequestDelegate next { get; set; }
        public OtherMiddleware(RequestDelegate _next)
        {
            next = _next;
        }
        public async Task Invoke(HttpContext context, ILogger<OtherMiddleware> logger)
        {
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
            //ya da 
            correlationId = context.Items["CorrelationId"].ToString();

            NLog.MappedDiagnosticsContext.Set("CorrelaltionId", correlationId); //NLog.config'deki MDC correlationId parametresini set ediyoruz Set fonk üzerinden
            logger.LogDebug("OtherMiddleware Log"); // DB'deki EventMessage alanına yazılıyor.

            await next(context);
        }
    }
}
