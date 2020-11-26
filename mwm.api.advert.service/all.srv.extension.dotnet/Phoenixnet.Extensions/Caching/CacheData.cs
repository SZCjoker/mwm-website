namespace Phoenixnet.Extensions.Caching
{
    public class CacheData<TData, TChecksum>
    {
        public TData Data { get; set; }
        public TChecksum Checksum { get; set; }
    }
}