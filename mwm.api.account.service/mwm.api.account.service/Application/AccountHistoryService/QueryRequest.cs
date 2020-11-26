using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountHistoryService
{
    public class QueryRequest
    {/// <summary>
     ///帳號 
     /// </summary>
        public string login_name { get; set; }
        /// <summary>
        /// 主功能選項
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 子項目-行為
        /// </summary>
        public string action { get; set; }
        /// <summary>
        /// 起始時間
        /// </summary>
        public string begin_date { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public string end_date { get; set; }

        public int page_offset { get; set; }

        public int page_size { get; set; }
    }
}
