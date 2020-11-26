using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MWM.API.Site.Service.Application.Common;
using Newtonsoft.Json;
using MWM.API.Site.Service.ModuleConfig;
using System;

namespace MWM.API.Site.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<AppSettings>(Configuration);
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor;
            });

            services.AddMvcCore()
                    .AddAuthorization()
                    .AddApiExplorer()
                    .AddFormatterMappings()
                    .AddDataAnnotations()
                    .AddJsonFormatters(options =>
                    {
                        options.Converters.Add(new LongValueConverter());
                        options.NullValueHandling = NullValueHandling.Ignore;
                    })
                    .AddCors(options =>
                    {
                        options.AddPolicy("CorsPolicy", o =>
                        {
                            o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                        });
                    });           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
//#if DEBUG
            app.UseGlobalExceptionMiddleware();
            //app.UseMiddleware<StackifyMiddleware.RequestTracerMiddleware>();
                        
//#endif

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseResponseCompression();
            app.UseMvc();
            app.ApplySwagger();
           
            
        }
    }
}