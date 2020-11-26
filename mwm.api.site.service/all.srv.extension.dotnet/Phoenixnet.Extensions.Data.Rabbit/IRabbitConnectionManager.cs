using System;
using RabbitMQ.Client;

namespace Phoenixnet.Extensions.Data.Rabbit
{
    /// <summary>
    /// RabbitMQ 連線管理
    /// </summary>
    public interface IRabbitConnectionManager : IDisposable
    {
        /// <summary>
        /// 取得 RabbitMQ 的連線
        /// </summary>
        IConnection Connection { get; }
    }
}