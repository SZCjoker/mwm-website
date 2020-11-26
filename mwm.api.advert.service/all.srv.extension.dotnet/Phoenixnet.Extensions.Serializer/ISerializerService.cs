namespace Phoenixnet.Extensions.Serializer
{
    public interface ISerializer
    {
        string Serialize<T>(T data);

        string Serialize(object data);

        T Deserialize<T>(string data);
    }
}