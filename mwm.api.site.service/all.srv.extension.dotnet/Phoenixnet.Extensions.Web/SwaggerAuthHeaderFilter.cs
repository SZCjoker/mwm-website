﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Phoenixnet.Extensions.Web
{
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
