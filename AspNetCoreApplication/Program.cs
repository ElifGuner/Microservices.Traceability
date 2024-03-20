using AspNetCoreApplication.Middlewares;
using Microsoft.AspNetCore.Builder;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

#region NLog Setup
builder.Logging.ClearProviders();
builder.Host.UseNLog();
#endregion


var app = builder.Build();

//yazd���m�z middleware'leri sisteme dahil ediyoruz.
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<OtherMiddleware>();


app.MapGet("/", (HttpContext context, ILogger<Program> logger) =>
{
    var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
    //ya da 
    correlationId = context.Items["CorrelationId"].ToString();

    NLog.MappedDiagnosticsContext.Set("CorrelaltionId", correlationId); //NLog.config'deki MDC correlationId parametresini set ediyoruz Set fonk �zerinden
    logger.LogDebug("Minimal API Log"); // DB'deki EventMessage alan�na yaz�l�yor.
});

app.Run();
