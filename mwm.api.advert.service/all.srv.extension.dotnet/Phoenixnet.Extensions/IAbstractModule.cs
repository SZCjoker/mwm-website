using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Phoenixnet.Extensions
{
    /// <summary>
    /// DI 模組
    /// </summary>
    public interface IAbstractModule
    {
        /// <summary>
        /// 設定模組內容
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        void Load(IServiceCollection services, IConfiguration configuration);
    }
}