using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Phoenixnet.Extensions.Serializer;

namespace Phoenixnet.Extensions.Web
{
    public class RequestResponseMiddlewareV1
    {
        private readonly ILogger<RequestResponseMiddlewareV1> _logger;
        private readonly RequestDelegate _next;
        private readonly ISerializer _serializer;

        /// <summary>
        /// </summary>
        /// <param name="next"></param>
        /// <param name="serializer"></param>
        /// <param name="logger"></param>
        public RequestResponseMiddlewareV1(RequestDelegate next, ISerializer serializer, ILogger<RequestResponseMiddlewareV1> logger)
        {
            _next = next;
            _serializer = serializer;
            _logger = logger;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.HttpContext.RequestAborted.IsCancellationRequested)
                    context.Response.HttpContext.RequestAborted.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException cp)
            {
                _logger.LogError(cp, "Cancel Operation");
            }
            catch (Exception ex)
            {
                var env = context.RequestServices.GetRequiredService<IHostingEnvironment>();
                _logger.LogError(ex, "發生未預期的錯誤");

                context.Response.ContentType = "application/json";

                if (env.IsDevelopment())
                    await context.Response.WriteAsync(_serializer.Serialize(new
                    {
                        code = "500",
                        type_name = GetType().Name,
                        message = ex.Message,
                        stack = ex.ToString()
                    }));
                else
                    await context.Response.WriteAsync(_serializer.Serialize(new
                    {
                        code = "500",
                        message = ex.Message
                    }));
            }
        }
    }

    public static class RequestResponseMiddlewareV1Extension
    {
        public static IApplicationBuilder UseRequestResponseMiddlewareV1(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseMiddlewareV1>();
        }

        public static IServiceCollection AddRequestResponseMiddlewareV1(this IServiceCollection service)
        {
            service.AddSingleton<RequestResponseMiddlewareV1>();
            return service;
        }
    }
}
