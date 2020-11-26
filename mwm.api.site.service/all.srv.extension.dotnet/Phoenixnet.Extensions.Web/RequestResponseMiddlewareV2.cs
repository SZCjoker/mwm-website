using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Phoenixnet.Extensions.Serializer;
using Phoenixnet.Extensions.Object;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Web
{
    public class RequestResponseMiddlewareV2
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseMiddlewareV2> _logger;
        private readonly ISerializer _serializer;

        public RequestResponseMiddlewareV2(RequestDelegate next, ILogger<RequestResponseMiddlewareV2> logger, ISerializer serializer)
        {
            _next = next;
            _logger = logger;
            _serializer = serializer;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.HttpContext.RequestAborted.IsCancellationRequested)
            {
                await _next(context);
            }

            // First, get the incoming requeste
            await FormatRequest(context.TraceIdentifier, $"{context.Connection.RemoteIpAddress}", context.Request);
            
            if (context.Request.HttpContext.RequestAborted.IsCancellationRequested)
            {
                await _next(context);
            }

            // Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            // Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                // ...and use that for the temporary response body
                context.Response.Body = responseBody;

                // Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                // Format the response from the server
                await FormatResponse(context.TraceIdentifier, context.Response);

                // Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task FormatRequest(string traceId, string remoteAddress, HttpRequest request)
        {
            // This line allows us to set the reader for the request back at the beginning of its stream.
            //很重要所以說三次,請擺在第一個！第一個！第一個
            request.EnableRewind();
            var body = request.Body;
            // We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            // ...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            //請務必務必務必＊＊＊歸零,很重要所以說三次
            request.Body.Seek(0, SeekOrigin.Begin);
            // We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            // ..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body = body;

            var content = _serializer.Serialize(new RequestEntry
            {
                Address = remoteAddress,
                Headers = request.Headers.ToDictionary(
                    e => $"{e.Key}",
                    e => string.Join("; ", e.Value)
                ),
                Body = bodyAsText
            });

            _logger.LogWarning(content);
        }

        private async Task FormatResponse(string traceId, HttpResponse response)
        {
            // We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            // ...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            // We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            var content = _serializer.Serialize(new ResponseEntry
            {
                Http = response.StatusCode,
                Headers = response.Headers.ToDictionary(
                    e => $"{e.Key}",
                    e => string.Join("; ", e.Value)
                ),
                Body = text
            });

            _logger.LogWarning(content);
        }
    }
    public static class RequestResponseMiddlewareV2Extension
    {
        public static IApplicationBuilder UseRequestResponseMiddlewareV2(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseMiddlewareV2>();
        }

        public static IServiceCollection AddRequestResponseMiddlewareV2(this IServiceCollection service)
        {
            service.AddSingleton<RequestResponseMiddlewareV2>();
            return service;
        }
    } 
}
