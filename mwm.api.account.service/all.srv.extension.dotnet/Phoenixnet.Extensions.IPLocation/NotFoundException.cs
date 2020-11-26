using System;

namespace Phoenixnet.Extensions.IPLocation
{
    public class NotFoundException : Exception
    {

        public NotFoundException(string name) : base(name)
        {
        }
    }
}
