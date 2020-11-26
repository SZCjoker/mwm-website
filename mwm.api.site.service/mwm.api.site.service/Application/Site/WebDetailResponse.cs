using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Site.Datatype
{
    public class WebDetailResponse
    {
        public long id { get; set; }
        public long account_id { get; set; }
        public string address { get; set; }
        public string mail { get; set; }
        public string logo { get; set; }  
        public string contact { get; set; }
        public List<string> newest_address { get; set; }
        public List<string> eternal_addres { get; set; }
        public string publish_msg_top { get; set; }
        public string publish_msg_bottom { get; set; }
        public int cdate { get; set; }
        public Int64 ctime { get; set; }
        public int udate { get; set; }
        public Int64 utime { get; set; }
        public int state { get; set; }
    }
}
