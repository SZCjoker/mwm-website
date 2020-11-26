using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Phoenixnet.Extensions.Handler
{
    public static class SettingHandler
    {
        public static IConfigurationRoot AddJsonFile()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .Build();

            return config;
        }
    }
}
