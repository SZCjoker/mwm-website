using Jil;
using Swifter.Json;
using System;

namespace Phoenixnet.Extensions.Serializer
{
    public class SwifterSerializer : ISerializer
    {
        public T Deserialize<T>(string data)
        {
            return JsonFormatter.DeserializeObject<T>(data); ;
        }

        public string Serialize<T>(T data)
        {
            return JsonFormatter.SerializeObject<T>(data);
        }

        public string Serialize(object data)
        {
            return JsonFormatter.SerializeObject(data);
        } 
    }
}