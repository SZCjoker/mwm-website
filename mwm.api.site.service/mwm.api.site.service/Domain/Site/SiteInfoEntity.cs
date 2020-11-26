using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Site
{
    public class SiteInfoEntity
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public string Address { get; set; }
        public string Mail { get; set; }
        public string Logo { get; set; }
        public string Contact { get; set; }
        public string NewestAdd { get; set; }
        public string EternalAdd { get; set; }
        public string PubMsgTop { get; set; }
        public string PubMsgBottom { get; set; }
        public int Cdate { get; set; }
        public Int64 Ctime { get; set; }
        public int Udate { get; set; }
        public Int64 Utime { get; set; }
        public Int16 State { get; set; }
    }
}
