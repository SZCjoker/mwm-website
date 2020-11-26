using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Phoenixnet.Extensions
{
    /// <summary>
    /// 模組設定擴充函式
    /// </summary>
    public static class UseModuleConfigExtension
    {
        /// <summary>
        /// Web 專案擴充模組
        /// </summary>
        /// <param name="webHostBuilder"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseModuleConfig(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureServices((builder, services) =>
            {
                foreach (Type type in Assembly.GetEntryAssembly().GetTypes().Where(t => typeof(IAbstractModule).IsAssignableFrom(t)))
                {
                    // 忽略標示為 [Obsolete] 的模組
                    if (type.GetCustomAttributes<ObsoleteAttribute>().Any())
                    {
                        continue;
                    }

                    var module = Activator.CreateInstance(type) as IAbstractModule;
                    module?.Load(services, builder.Configuration);
                }
            });

            return webHostBuilder;
        }

        /// <summary>
        /// Console 專案擴充模組
        /// </summary>
        /// <param name="service"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection UserModuleConfig(this IServiceCollection service, IConfiguration configuration)
        {
            foreach (Type type in Assembly.GetEntryAssembly().GetTypes().Where(t => typeof(IAbstractModule).IsAssignableFrom(t)))
            {
                // 忽略標示為 [Obsolete] 的模組
                if (type.GetCustomAttributes<ObsoleteAttribute>().Any())
                {
                    continue;
                }

                var module = Activator.CreateInstance(type) as IAbstractModule;
                module?.Load(service, configuration);
            }

            return service;
        }
    }
}