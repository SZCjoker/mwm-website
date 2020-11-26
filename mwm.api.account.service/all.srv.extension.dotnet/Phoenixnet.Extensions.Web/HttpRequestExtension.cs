using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Web
{
    public static class HttpRequestExtension
    {
        public static async Task<string> ReadBodyAsync(this HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                if (request.Body.CanSeek)
                {
                    request.Body.Seek(0, SeekOrigin.Begin);
                }

                return body;
            }
        }

        public static string GetHeaderAddress(this HttpRequest request)
        {
            if (request == null) return string.Empty;
            if (request.Headers["X-Forwarded-For"].Count == 0) return request.Host.ToString();

            return request.Headers["X-Forwarded-For"].ToString();
        }

        public static int GetSize(this HttpRequest request)
        {
            if (request == null) return 0;

            if (request.ContentLength.HasValue)
                return (int)request.ContentLength;
            else
                return 0;
        }
    }
}
