using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Site.Datatype
{
    public class WebDetailRequest
    {   /// <summary>
        ///站點管理資料ID 
        /// </summary>
        public Int64 id { get; set; }       
        public string address { get; set; } = null;
        public string mail { get; set; } = null;
        public string logo { get; set; } = null;
        public string contact { get; set; } = null;
        public string newest_address { get; set; } = null;
        public string eternal_address { get; set; } = null;
        /// <summary>
        ///公告訊息-上 
        /// </summary>
        public string publish_msg_top { get; set; } = null;
        /// <summary>
        /// 公告訊息-下
        /// </summary>
        public string publish_msg_bottom { get; set; } = null;
    }



   
}
