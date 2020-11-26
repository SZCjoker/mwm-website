using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Template
{
    public class QueryRequest
    {
        public string id { get; set; }
        public string img { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public Int32 cdate { get; set; }
        public Int64 ctime { get; set; }
        public Int16 state { get; set; }
    }
}
