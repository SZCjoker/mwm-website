using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using Phoenixnet.Extensions.Message;

namespace Phoenixnet.Extensions.Caching.CsRedis
{
    /// <summary>
    /// CsRedis 連線設定
    /// </summary>
    public static class CsRedisConnectionExtension
    {
        /// <summary>
        /// CsRedis 單台
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="config"></param>
        public static void UseCsRedis(this IServiceCollection serviceCollection, string config)
        {
            serviceCollection.AddSingleton(s =>
                    {
                        var csRedis = new CSRedisClient(config);
                        RedisHelper.Initialization(csRedis);
                        return RedisHelper.Instance;
                    }
                ).AddTransient<IDistributedCache, CSRedisCache>()
                .AddTransient<ICacheMget, CsRedisSimulationMget>()
                .AddTransient<IPubSubService, CsRedisPubSub>();
        }

     

        /// <summary>
        /// 创建redis哨兵访问类(Redis Sentinel)
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="connectionString">mymaster,password=123456,poolsize=50,connectTimeout=200,ssl=false</param>
        /// <param name="sentinels">哨兵节点，如：ip1:26379、ip2:26379</param>
        public static void UseCsRedisSentinel(this IServiceCollection serviceCollection,string connectionString, string[] sentinels)
        {
            serviceCollection
                .AddSingleton(s =>
                    {
                        var csRedis = new CSRedisClient(connectionString,sentinels);
                        RedisHelper.Initialization(csRedis);
                        return RedisHelper.Instance;
                    }
                ).AddTransient<IDistributedCache, CSRedisCache>()
                .AddTransient<ICacheMget, CsRedisSimulationMget>()
                .AddTransient<IPubSubService, CsRedisPubSub>();
        }
    }
}