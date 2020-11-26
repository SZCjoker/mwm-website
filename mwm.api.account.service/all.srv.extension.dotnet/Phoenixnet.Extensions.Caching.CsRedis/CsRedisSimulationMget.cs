using CSRedis;
using Phoenixnet.Extensions.Method;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Caching.CsRedis
{
    /// <summary>
    /// 針對Redis Cluster 無提供Ｍget的方法.
    /// 利用Redis Get的方式採迭代的方法.達到Mget的需求
    /// </summary>
    public class CsRedisSimulationMget : ICacheMget 
    { 
        private readonly CSRedisClient _manager;

        public CsRedisSimulationMget(CSRedisClient manger)
        {
            _manager = manger;
        }

        public async Task<Dictionary<string, byte[]>> MGetAsync(string[] key)
        {
            var result = new Dictionary<string, byte[]>();
            await key.EachAsync(async (s, i) =>
            {
                var results = await _manager.GetAsync(s);
                result.Add(s, string.IsNullOrEmpty(results) ? null : Encoding.UTF8.GetBytes(results));
            });
            return result;
        }

        public Dictionary<string, byte[]> MGet(string[] key)
        {
            var result = new Dictionary<string, byte[]>();
            key.Each((i, s) =>
            {
                var results = _manager.Get(s);
                result.Add(s, string.IsNullOrEmpty(results) ? null : Encoding.UTF8.GetBytes(results));
            });
            return result;
        }
    }
}