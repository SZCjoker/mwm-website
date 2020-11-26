using CSRedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Caching.CsRedis
{
    public class CsRedisMget : ICacheMget 
    {
        private readonly CSRedisClient _manager;

        public CsRedisMget(CSRedisClient manger)
        {
            _manager = manger;
        }

        public async Task<Dictionary<string, byte[]>> MGetAsync(string[] key)
        {
            var results = await _manager.MGetAsync(key);
            var enumerable = key.Zip(results, (n, w) => new { key = n, val = w });
            return enumerable.ToDictionary(s => s.key, d => string.IsNullOrEmpty(d.val) ? (byte[])null : Encoding.UTF8.GetBytes(d.val));
        }

        public Dictionary<string, byte[]> MGet(string[] key)
        {
            var results = _manager.MGet(key);
            var enumerable = key.Zip(results, (n, w) => new { key = n, val = w });
            return enumerable.ToDictionary(s => s.key, d => string.IsNullOrEmpty(d.val) ? (byte[])null : Encoding.UTF8.GetBytes(d.val));
        }
    }
}