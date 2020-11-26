using CSRedis;
using Phoenixnet.Extensions.Message;
using System;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Caching.CsRedis
{
    public class CsRedisPubSub : IPubSubService 
    {
        private readonly CSRedisClient _manager;

        public CsRedisPubSub(CSRedisClient manger)
        {
            _manager = manger;
        }

        public async Task<long> PublishAsync(string channel, string message)
        {
            return await _manager.PublishAsync(channel, message);
        }

        public async Task<long> PublishLPushBLPopAsync(string channel, string message)
        {
            return await _manager.LPushAsync(channel, message);
        }

        public Task Subscribe(string channel, Action<string, string> onMessage)
        {
            var subscribePara = new ValueTuple<string, Action<CSRedisClient.SubscribeMessageEventArgs>>
            { Item1 = channel, Item2 = (msg) => { onMessage?.Invoke(channel, msg.Body); } };

            _manager.Subscribe(subscribePara);

            return Task.CompletedTask;
        }

        public Task SubscribeLPushBLPop(string channel, Action<string> onMessage)
        {
            _manager.SubscribeList(channel, onMessage);
            return Task.CompletedTask;
        }
    }
}