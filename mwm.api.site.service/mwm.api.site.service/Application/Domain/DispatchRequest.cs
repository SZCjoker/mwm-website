using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Domain
{
    public class DispatchRequest
    {
        public long id { get; set; }
        /// <summary>
        /// 流量主ID-必要
        /// </summary>
        public long account_id { get; set; }
        public string domain_name { get; set; }
        public int cdate { get; set; }
        public Int64 ctime { get; set; }
        public int udate { get; set; }
        public Int64 utime { get; set; }
        public int state { get; set; }
    }
}
