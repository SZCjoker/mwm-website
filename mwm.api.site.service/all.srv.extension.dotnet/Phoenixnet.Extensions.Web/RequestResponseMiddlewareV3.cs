using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Phoenixnet.Extensions.Serializer;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Web
{
    public class RequestResponseMiddlewareV3
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseMiddlewareV2> _logger;
        private readonly ISerializer _serializer;

        public RequestResponseMiddlewareV3(RequestDelegate next, ILogger<RequestResponseMiddlewareV2> logger, ISerializer serializer)
        {
            _next = next;
            _logger = logger;
            _serializer = serializer;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            if (context.Request.HttpContext.RequestAborted.IsCancellationRequested)
            {
                await _next(context);
            }

            // Leave the body open so the next middleware can read it.
            using (var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 500,
                leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                // Do some processing with body…

                // Reset the request body stream position so the next middleware can read it
                context.Request.Body.Position = 0;
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }

    public static class RequestResponseMiddlewareV3Extension
    {
        public static IApplicationBuilder UseRequestResponseMiddlewareV3(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseMiddlewareV3>();
        }

        public static IServiceCollection AddRequestResponseMiddlewareV3(this IServiceCollection service)
        {
            service.AddSingleton<RequestResponseMiddlewareV3>();
            return service;
        }
    }
}
