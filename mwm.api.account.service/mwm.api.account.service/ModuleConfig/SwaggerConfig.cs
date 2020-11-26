using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phoenixnet.Extensions;
using Phoenixnet.Extensions.Web;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MWM.API.Account.Service.ModuleConfig
{
    public class SwaggerConfig : IAbstractModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {   
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.DescribeAllEnumsAsStrings();
                c.CustomSchemaIds(type => type.FullName);
                c.OperationFilter<SwaggerAuthHeaderFilter>();
                c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MWM.API.Account.Service.xml"));
                c.SwaggerDoc("v1", new Info { Title = "MWM.API.Account.Service", Version = "v1" });
            });
        }
    }

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

    public class SwaggerAuthHeaderFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = (context.ApiDescription.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo.CustomAttributes;
            var isAuthorized = filterPipeline.Any(x => x.AttributeType.Equals(typeof(AuthorizeAttribute)));
            var isAllowAnonymous = filterPipeline.Any(x => x.AttributeType.Equals(typeof(AllowAnonymousAttribute)));

            if (!isAllowAnonymous && isAuthorized)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<IParameter>();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Description = "OAuth Token",
                    Required = true,
                    Type = "string",
                    Default = "Bearer "
                });
            }
        }
    }

}
