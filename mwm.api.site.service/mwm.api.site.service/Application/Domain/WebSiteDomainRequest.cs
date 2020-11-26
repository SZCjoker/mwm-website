using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Domain
{
    public class WebSiteDomainRequest
    {   /// <summary>
        ///網站網域資料ID 
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 流量主ID
        /// </summary>
        public long account_id { get; set; }
        /// <summary>
        /// 系統分派網域ID
        /// </summary>
        public long dispatch_domain_id { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public long template_id { get; set; }
        /// <summary>
        /// 流量主公開的網域名稱
        /// </summary>
        public string public_domain { get; set; } = null;
        /// <summary>
        /// 網站名稱
        /// </summary>
        public string website_name { get; set; } = null;
        /// <summary>
        /// 網站描述
        /// </summary>
        public string website_desc { get; set; } = null;
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string keyword { get; set; } = null;
        /// <summary>
        /// 可用狀態
        /// </summary>
        public int state { get; set; }
    }
}
