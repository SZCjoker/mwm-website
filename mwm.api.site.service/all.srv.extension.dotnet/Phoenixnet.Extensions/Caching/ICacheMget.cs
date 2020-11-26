using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Caching
{
    /// <summary>
    /// 多筆取值服務
    /// </summary>
    public interface ICacheMget
    {
        Task<Dictionary<string, byte[]>> MGetAsync(string[] key);

        Dictionary<string, byte[]> MGet(string[] key);
    }
}