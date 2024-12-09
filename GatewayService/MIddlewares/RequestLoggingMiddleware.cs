using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GatewayService.MIddlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopWatch = Stopwatch.StartNew();

            // Request logging
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

            // Skipping request to next element in pipeline
            await _next(context);

            stopWatch.Stop();

            // Response logging
            _logger.LogInformation($"Response: {context.Response.StatusCode} Time: {stopWatch.ElapsedMilliseconds}ms");
        }
    }
}
