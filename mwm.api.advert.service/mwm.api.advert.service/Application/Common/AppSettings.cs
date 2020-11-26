namespace MWM.API.Advert.Service.Application.Common
{
    public class AppSettings
    {
        public Storage storage { get; set; }
        public Application application { get; set; }
        public Logging Logging { get; set; }
        public Ipratelimiting IpRateLimiting { get; set; }
    }

    public class Storage
    {
        public Mysql mysql { get; set; }
        public Redis redis { get; set; }
        public Mongodb mongodb { get; set; }
        public Rabbitmq rabbitmq { get; set; }
        public Elasticsearch elasticsearch { get; set; }
    }

    public class Mysql
    {
        public Readonly readOnly { get; set; }
        public Readwrite readWrite { get; set; }
    }

    public class Readonly
    {
        public string connection { get; set; }
        public string provider { get; set; }
    }

    public class Readwrite
    {
        public string connection { get; set; }
        public string provider { get; set; }
    }

    public class Redis
    {
        public string master { get; set; }
        public string slave { get; set; }
    }

    public class Mongodb
    {
        public string connection { get; set; }
        public string name { get; set; }
    }

    public class Rabbitmq
    {
        public string connection { get; set; }
        public int poolSize { get; set; }
        public Channel channel { get; set; }
    }

    public class Channel
    {
        public string success { get; set; }
        public string retry { get; set; }
    }

    public class Elasticsearch
    {
        public string host { get; set; }
        public string index { get; set; }
    }

    public class Application
    {
        public bool hashCode { get; set; }
        public bool swagger { get; set; }
        public Slack slack { get; set; }
        public Video video { get; set; }
    }

    public class Slack
    {
        public string channel { get; set; }
        public string url { get; set; }
    }

    public class Video
    {
        public bool use_cache { get; set; }
        public int cache_minute { get; set; }
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }

    public class Ipratelimiting
    {
        public bool EnableEndpointRateLimiting { get; set; }
        public bool StackBlockedRequests { get; set; }
        public string RealIpHeader { get; set; }
        public string ClientIdHeader { get; set; }
        public int HttpStatusCode { get; set; }
        public Generalrule[] GeneralRules { get; set; }
    }

    public class Generalrule
    {
        public string Endpoint { get; set; }
        public string Period { get; set; }
        public int Limit { get; set; }
    }

}