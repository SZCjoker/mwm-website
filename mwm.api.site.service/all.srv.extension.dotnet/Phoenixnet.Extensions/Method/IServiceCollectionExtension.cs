using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Phoenixnet.Extensions.Method
{
    /// <summary>
    /// 模組設定擴充函式
    /// </summary>
    public static class IServiceCollectionExtension
    {
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

                var module = (IAbstractModule)Activator.CreateInstance(type);
                module.Load(service, configuration);
            }

            return service;
        }
    }
}