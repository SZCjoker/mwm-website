using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phoenixnet.Extensions;
using Phoenixnet.Extensions.Web;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace MWM.API.Advert.Service.ModuleConfig
{
    public class SwaggerConfig : IAbstractModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                //c.TagActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
                //c.OrderActionsBy(api => api.HttpMethod);
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.DescribeAllEnumsAsStrings();
                c.CustomSchemaIds(type => type.FullName);
                c.OperationFilter<SwaggerAuthHeaderFilter>();
                c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MWM.API.Advert.Service.xml"));
                c.SwaggerDoc("v1", new Info { Title = "MWM.API.Advert.Service", Version = "v1" });
            });
        }
    }
}
