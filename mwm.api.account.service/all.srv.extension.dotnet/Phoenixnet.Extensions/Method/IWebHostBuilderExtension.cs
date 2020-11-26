using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace Phoenixnet.Extensions.Method
{
    public static class IWebHostBuilderExtension
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

                    var module = (IAbstractModule)Activator.CreateInstance(type);
                    module.Load(services, builder.Configuration);
                }
            });

            return webHostBuilder;
        }
    }
}