using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.AccountHistory
{
    public class QueryAllEntity
    {   /// <summary>
     ///帳號 
     /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 主功能選項
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 子項目-行為
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 起始時間
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public string EndDate { get; set; }

        public int PageOffset { get; set; }

        public int PageSize { get; set; }
    }
}
