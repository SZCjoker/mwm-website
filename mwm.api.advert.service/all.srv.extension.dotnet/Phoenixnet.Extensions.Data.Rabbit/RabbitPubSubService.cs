using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Phoenixnet.Extensions.Message;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Data.Rabbit
{
    /// <summary>
    /// RabbitMQ 訊息處理
    /// </summary>
    public class RabbitPubSubService : IPubSubService, IDisposable
    {
        /// <summary>
        /// 連線管理
        /// </summary>
        private readonly IRabbitConnectionManager _manager;

        /// <summary>
        /// 日誌
        /// </summary>
        private readonly ILogger<RabbitPubSubService> _logger;

        public RabbitPubSubService(IRabbitConnectionManager manager, ILogger<RabbitPubSubService> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public Task<long> PublishAsync(string channel, string message)
        {
            if (string.IsNullOrWhiteSpace(channel)) throw new ArgumentException(nameof(channel));
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException(nameof(message));

            var data = Encoding.UTF8.GetBytes(message);

            // 建立 channel
            using (var model = _manager.Connection.CreateModel())
            {
                // 宣告交換機
                // producer 與 consumer 都應宣告相同的屬性
                model.ExchangeDeclare(
                    exchange: channel,
                    type: ExchangeType.Direct,
                    autoDelete: false,
                    durable: true,
                    arguments: null);

                // 發佈發訊
                model.BasicPublish(
                    exchange: channel,
                    routingKey: channel,
                    body: data,
                    mandatory: true,
                    basicProperties: null);

                _logger.LogDebug($"傳送訊息 {message}");
            }

            return Task.FromResult(1L);
        }

        public Task Subscribe(string channel, Action<string, string> onMessage)
        {
            if (string.IsNullOrWhiteSpace(channel)) throw new ArgumentException(nameof(channel));
            if (onMessage == null) throw new ArgumentException(nameof(onMessage));

            using (var model = _manager.Connection.CreateModel())
            {
                // 宣告交換機
                // producer 與 consumer 都應宣告相同的屬性
                model.ExchangeDeclare(
                    exchange: channel,
                    type: ExchangeType.Direct,
                    autoDelete: false,
                    durable: true,
                    arguments: null);

                // 建立 queue
                var queue = model.QueueDeclare(
                    queue: channel,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                model.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                // 綁定要處理的 queue
                model.QueueBind(queue: queue.QueueName, exchange: channel, routingKey: channel);

                var consumer = new EventingBasicConsumer(model);

                consumer.Received += (sender, args) =>
                {
                    var c = (EventingBasicConsumer)sender;
                    var m = c.Model;

                    try
                    {
                        var data = args.Body;
                        var message = Encoding.UTF8.GetString(data);

                        // 處理訊息
                        onMessage?.Invoke(channel, message);

                        // 回應成功
                        m.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"處理訊息發生錯誤: {e}");

                        // Requeue
                        m.BasicNack(deliveryTag: args.DeliveryTag, multiple: false, requeue: true);
                    }
                };

                // 開始接收訊息，設置為手動回應完成
                model.BasicConsume(
                    queue: queue.QueueName,
                    autoAck: false,
                    consumer: consumer);

                SpinWait.SpinUntil(() => false);
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _manager != null)
            {
                _manager.Dispose();
            }
        }

        public Task<long> PublishLPushBLPopAsync(string channel, string message)
        {
            throw new NotImplementedException();
        }

        public Task SubscribeLPushBLPop(string channel, Action<string> onMessage)
        {
            throw new NotImplementedException();
        }
    }
}