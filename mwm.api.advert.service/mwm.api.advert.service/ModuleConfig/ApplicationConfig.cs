using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Phoenixnet.Extensions;
using Phoenixnet.Extensions.Caching;
using Phoenixnet.Extensions.Caching.CsRedis;
using Phoenixnet.Extensions.Data.MySql;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Serializer;
using System;
using System.Collections.Generic;

namespace MWM.API.Advert.Service.ModuleConfig
{
    /// <summary>
    /// 主要服務模組
    /// </summary>
    public class ApplicationConfig : IAbstractModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.UseCsRedis(configuration.GetValue<string>("storage:redis:master"));
            services.AddSingleton<IDbFactory, MySqlDbFactory>()
                    .AddSingleton<ISerializer, SwifterSerializer>()
                    .AddTransient<ICachingService, CsRedisCachingService>()
                    .AddSingleton<IGenerateId>(s=> new SnowflakeHandler(Environment.MachineName));
 
            services.AddResponseCompression(options =>
            {
                IEnumerable<string> MimeTypes = new[]
                {
                     "text/plain",
                     "application/json"
                 };
                options.EnableForHttps = true;
                options.MimeTypes = MimeTypes;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            }); 

            services.AddStartupTask<WarmupServicesStartupTask>().TryAddSingleton(services);
        }
    }
}