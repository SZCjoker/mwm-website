using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Phoenixnet.Extensions.Web
{
    public static class SwaggerExtension
    {
        public static void ApplySwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1.0.0");
                opt.DocExpansion(DocExpansion.None);
                opt.DisplayRequestDuration();
                opt.DisplayOperationId();
            });
        }
    }
}
