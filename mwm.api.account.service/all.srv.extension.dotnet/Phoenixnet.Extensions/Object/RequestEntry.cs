using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenixnet.Extensions.Object
{
    public struct RequestEntry
    {
        public string Address { get; set; } 
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
    } 
}
