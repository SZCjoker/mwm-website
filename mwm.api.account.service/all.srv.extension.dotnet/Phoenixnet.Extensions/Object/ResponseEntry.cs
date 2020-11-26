using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenixnet.Extensions.Object
{ 
    public struct ResponseEntry
    {
        public int Http { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
    }
}
