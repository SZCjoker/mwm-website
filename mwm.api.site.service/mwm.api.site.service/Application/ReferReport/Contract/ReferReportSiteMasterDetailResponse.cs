using System.Collections.Generic;

namespace MWM.API.Site.Service.Application.ReferReport.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class ReferReportSiteMasterDetailResponse
    {
        /// <summary>
        /// 流量主id
        /// </summary>
        public int account_id { get; set; }
        /// <summary>
        /// 流量主登入名稱
        /// </summary>
        public string login_name { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public int date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <summary>
        /// 網站名稱
        /// </summary>
        public string refer_url { get; set; }
        /// <summary>
        /// 流量
        /// </summary>
        public decimal flow { get; set; }
        /// <summary>
        /// 觀看次數
        /// </summary>
        public long visit_data { get; set; }

    }


}