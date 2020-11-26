namespace MWM.API.Site.Service.Application.ReferReport.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class ReferReportAllResponse
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
        /// 流量
        /// </summary>
        public decimal flow { get; set; }
        /// <summary>
        /// 觀看次數
        /// </summary>
        public long visit_data { get; set; }
        /// <summary>
        /// 擁有網域數
        /// </summary>
        public int domain_cnt { get; set; }
    }
}