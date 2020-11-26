using System.IO;

namespace Phoenixnet.Extensions.IPLocation
{
    public class InvalidDatabaseException : IOException
    {
        public InvalidDatabaseException(string message) : base(message)
        {
        }
    }
}