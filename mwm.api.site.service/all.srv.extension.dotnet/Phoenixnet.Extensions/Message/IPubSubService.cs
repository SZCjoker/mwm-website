using System;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Message
{
    /// <summary>
    /// Pub/Sub 服務
    /// </summary>
    public interface IPubSubService
    {
        /// <summary>
        /// 發送訊息
        /// </summary>
        /// <param name="channel">訂閱頻道</param>
        /// <param name="message">訊息</param>
        /// <returns></returns>
        Task<long> PublishAsync(string channel, string message);

        /// <summary>
        /// 發送訊息
        /// </summary>
        /// <param name="channel">訂閱頻道</param>
        /// <param name="message">訊息</param>
        /// <returns></returns>
        Task<long> PublishLPushBLPopAsync(string channel, string message);

        /// <summary>
        /// 接收訊息
        /// </summary>
        /// <param name="channel">訂閱頻道</param>
        /// <param name="onMessage">處理訊息</param>
        Task Subscribe(string channel, Action<string, string> onMessage);

        /// <summary>
        /// 接收訊息
        /// </summary>
        /// <param name="channel">訂閱頻道</param>
        /// <param name="onMessage">處理訊息</param>
        Task SubscribeLPushBLPop(string channel, Action<string> onMessage);
    }
}