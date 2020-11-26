using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Site.Service.Application.Common;
using Phoenixnet.Extensions.Object;
using Phoenixnet.Extensions.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Domain
{
    public class SyncDomain : ISyncDomain
    {

        private readonly ILogger<DomainService> _logger;
        private readonly ISerializer _serializer;
        private readonly IOptions<AppSettings> _settings;
        private readonly ISync _syncs;


        public SyncDomain(IOptions<AppSettings> settings,
                            ISerializer serializer,
                            ISync syncs,
                            ILogger<DomainService> logger)
        {
            _settings = settings;
            _serializer = serializer;
            _syncs = syncs;
            _logger = logger;
        }
               

        public async Task<BasicResponse> SyncDomian(HttpMethod method, string sid, string Domain, int state = 0)
        {
            Domain = Domain.ToLower();
            _logger.LogInformation($" Sync HttpMethod:{method},SID:{sid},Domain:{Domain},State:{state} ");
            var url = $"{_settings.Value.application.api.b2bapiDomainsyns}?sid={sid}";
             //var url = $"http://localhost:54702/api/Domain?sid={sid}";
            try
            {
                var response = await _syncs.SyncAsync(method, url, sid, Domain, state);
                var result = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($" Sync Result:{result}");
                return _serializer.Deserialize<BasicResponse>(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" Sync Exception:{ex.Message} ");
                return new BasicResponse { code = 300, desc = "同步域名失敗" };
            }
        }

        public class DomainAsync : ISync
        {
            private readonly IHttpClientFactory _clientFactory;
            private readonly ISerializer _serializer;

            public DomainAsync(IHttpClientFactory clientFactory, ISerializer serializer)
            {
                _clientFactory = clientFactory;
                _serializer = serializer;
            }

            public async Task<HttpResponseMessage> SyncAsync(HttpMethod method, string url, string sid, string host, int state)
            {
                if (method == HttpMethod.Delete)
                {
                    var ApiData = new { host = host, state = state };
                    var ApiJson = _serializer.Serialize(ApiData);
                    using (var client = _clientFactory.CreateClient("sync"))
                    {
                        client.Timeout = TimeSpan.FromMilliseconds(30000);
                        client.BaseAddress = new Uri(url);
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, string.Empty)
                        { Content = new StringContent(ApiJson, Encoding.UTF8, "application/json") };
                        return await client.SendAsync(request);
                    }
                }
                else
                {
                    var ApiData = new { host = host, state = state };
                    var ApiJson = _serializer.Serialize(ApiData);
                    using (var client = _clientFactory.CreateClient("sync"))
                    {
                        client.Timeout = TimeSpan.FromMilliseconds(30000);
                        using (var SyncContent = new StringContent(ApiJson, Encoding.UTF8, "application/json"))
                        {
                            return await client.PostAsync(url, SyncContent);
                        }
                    };
                }
            }
        }

    }
}
