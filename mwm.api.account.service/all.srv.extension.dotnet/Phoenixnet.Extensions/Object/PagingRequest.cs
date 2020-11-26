using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenixnet.Extensions.Object
{
    public struct PagingRequest
    {
        public Int16 index { get; set; }
        public Int16 size { get; set; }
        public String order { get; set; }
    }
}
