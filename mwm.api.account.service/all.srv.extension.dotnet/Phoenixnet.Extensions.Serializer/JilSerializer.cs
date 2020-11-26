using Jil;

namespace Phoenixnet.Extensions.Serializer
{
    public class JilSerializer : ISerializer
    {
        public T Deserialize<T>(string data)
        {
            return JSON.Deserialize<T>(data);
        }

        public string Serialize<T>(T data)
        {
            return JSON.Serialize<T>(data);
        }

        public string Serialize(object data)
        {
            return JSON.SerializeDynamic(data);
        }
    }
}