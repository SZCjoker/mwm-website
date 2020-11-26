namespace Phoenixnet.Extensions.Data
{
    public class DbSetting
    {
        public string Connection { get; set; }
        public string DbProvider { get; set; }
        public int Timeout { get; set; } = 30;
    }
}