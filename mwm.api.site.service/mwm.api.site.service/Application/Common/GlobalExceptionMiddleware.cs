using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MWM.API.Site.Service.Application.Common.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Common
{
    public class GlobalExceptionMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleException(context, ex);
                return;
            }

        }


        private async ValueTask HandleException(HttpContext context,Exception ex)
        {
            _logger.LogError($"\r\n[Exception] From : [{context.Request.Method}]{context.Request.Path} | Type : {ex.GetType().FullName} | Message : {ex.Message}\r\n StackTrace : {ex.StackTrace}");
            context.Response.Headers.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex is IException ? (ex as IException).StatusCode : (int)HttpStatusCode.InternalServerError;
            var errorResponse = new
            {
                Code = ex is IException ? (ex as IException).Code : ex.GetType().FullName,
                Message = ex is IException ? (ex as IException).ErrorMessage : ex.Message,
                Ticks = DateTimeOffset.UtcNow.Ticks
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        
        }
    }

    public static class GlobalExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
