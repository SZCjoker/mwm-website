using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Domain
{
    public class DomainResponse
    {
        public long ?id { get; set; }
        public string domain_name { get; set; }
        public int ?cdate { get; set; }
        public Int64 ?ctime { get; set; }
        public int ?udate { get; set; }
        public Int64 ?utime { get; set; }
        public int ?state { get; set; }
    }
}
