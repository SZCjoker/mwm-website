using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Domain
{
    public class VideoDomainRequest
    {
        /// <summary>
        ///影片網域資料ID 
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 撥放影片域名
        /// </summary>
        public string domain_name { get; set; }
        /// <summary>
        /// 域名可用狀態
        /// </summary>
        public int state { get; set; }
    }

}
