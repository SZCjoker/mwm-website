using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Phoenixnet.Extensions.IPLocation
{
    public static class IPLocationExtension
    {
        public static void UseIPLocation(this IServiceCollection serviceCollection, string ip_data)
        {
            
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string ipdbFileName = Path.Combine(assemblyFolder,ip_data);
            serviceCollection
                .AddSingleton(s =>new City(ipdbFileName))
                .AddScoped<IIPTransferRegion,IPTransferRegionImp>();
            
            
        }
    }
}