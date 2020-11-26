using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Phoenixnet.Extensions.Data.Rabbit
{
    /// <summary>
    /// RabbitMQ 連線管理
    /// </summary>
    public class RabbitConnectionManager : IRabbitConnectionManager
    {
        /// <summary>
        /// 連線池大小
        /// </summary>
        private readonly int _poolSize;

        /// <summary>
        /// 設定檔
        /// </summary>
        private readonly IConfiguration _configuration;

        private readonly ILogger<RabbitConnectionManager> _logger;

        /// <summary>
        /// 連線池
        /// </summary>
        private readonly List<IConnection> _pooledConnections = new List<IConnection>();

        private int _index;

        public RabbitConnectionManager(IConfiguration configuration, ILogger<RabbitConnectionManager> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _poolSize = configuration.GetValue<int>("storage:rabbitmq:poolSize");
        }

        /// <inheritdoc />
        public IConnection Connection
        {
            get
            {
                lock (_pooledConnections)
                {
                    try
                    {
                        return Next();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"{e}");
                        throw;
                    }
                }
            }
        }

        private IConnection Next()
        {
            IConnection connection;

            // 建立新連線條件:
            // 1. 連線池未滿
            // 2. 取得連線失敗
            // 3. 連線已關閉
            if (_pooledConnections.Count < _poolSize)
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(_configuration.GetValue<string>("storage:rabbitmq:connection")),
                };

                connection = factory.CreateConnection();

                _pooledConnections.Add(connection);
                return connection;
            }

            // 取得連線
            connection = _pooledConnections[_index++ % _pooledConnections.Count];

            if (connection.IsOpen)
            {
                return connection;
            }
            else
            {
                // 移除已關閉連線
                _pooledConnections.Remove(connection);
                return Next();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 關閉所有已開啟連線
                while (_pooledConnections.Count > 0)
                {
                    var connection = _pooledConnections[0];
                    _pooledConnections.Remove(connection);
                    connection.Close();
                    connection?.Dispose();
                }
            }
        }
    }
}