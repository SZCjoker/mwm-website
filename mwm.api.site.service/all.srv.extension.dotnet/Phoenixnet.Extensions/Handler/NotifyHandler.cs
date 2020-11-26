using Slack.Webhooks;
using System;
using System.Collections.Generic;

namespace Phoenixnet.Extensions.Handler
{
    public class NotifyParams
    {
        public string ChannelUrl { get; set; }
        public string ChannelName { get; set; }
        public string Title { get; set; }
        public string Client { get; set; }
        public Dictionary<string, string> AttachData { get; set; } = new Dictionary<string, string>();
    }

    public interface INotifyHandler
    {
        void Run(NotifyParams param);
    }

    public class SlackNotifyHandler : INotifyHandler
    {
        public void Run(NotifyParams param)
        {
            var slackClient = new SlackClient(param.ChannelUrl);

            var slackMessage = new SlackMessage
            {
                Channel = param.ChannelName,
                Text = param.Title,
                IconEmoji = Emoji.Ghost,
                Username = param.Client,
                Markdown = false
            };

            var fields = new List<SlackField>();

            foreach (KeyValuePair<string, string> item in param.AttachData)
            {
                fields.Add(new SlackField
                {
                    Title = item.Key,
                    Value = item.Value
                });
            }

            var slackAttachment = new SlackAttachment
            {
                Color = "#D00000",
                Fields = fields
            };

            slackMessage.Attachments = new List<SlackAttachment> { slackAttachment };
            slackClient.Post(slackMessage);
        }
    }

    public static class NotifyHandler
    {
        public static (bool hasException, T response) TryCatch<T>(Func<T> method, INotifyHandler handler, string channelName, string channelUrl, string json, string client)
        {
            try
            {
                var result = method();
                return (false, result);
            }
            catch (Exception ex)
            {
                var param = new NotifyParams()
                {
                    ChannelName = channelName,
                    ChannelUrl = channelUrl,
                    Title = method.Target.ToString(),
                    Client = client
                };
                param.AttachData.Add(method.Method.Name, json);
                param.AttachData.Add("ex", ex.Message);

                handler.Run(param);

                return (true, default(T));
            }
        }
    }
}
