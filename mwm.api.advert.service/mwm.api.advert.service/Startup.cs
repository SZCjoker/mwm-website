using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MWM.API.Advert.Service.Application.Common;
using Newtonsoft.Json;
using Phoenixnet.Extensions.Web;
using StackifyMiddleware;

namespace MWM.API.Advert.Service
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
            services.Configure<AppSettings>(Configuration);
            services.Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.XForwardedFor; });
            services.AddMvcCore()
                .AddAuthorization()
                .AddControllersAsServices()
                .AddApiExplorer()
                .AddFormatterMappings()
                .AddDataAnnotations()
                .AddJsonFormatters(options => { options.NullValueHandling = NullValueHandling.Ignore;
                                   options.Converters.Add(new LongValueConveter()); })
                .AddCors(options => { options.AddPolicy("CorsPolicy", o => { o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials(); }); });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP trafficMasterRequest pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
#if DEBUG
            //app.UseMiddleware<RequestTracerMiddleware>();
            app.UseGlobalExceptionMiddleware();
#endif

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseCors("CorsPolicy");
            app.UseResponseCompression();
            app.UseAuthentication();
            app.UseMvc();
            app.ApplySwagger();
        }
    }
}