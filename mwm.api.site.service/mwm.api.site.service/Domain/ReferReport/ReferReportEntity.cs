namespace MWM.API.Site.Service.Domain.ReferReport
{
    /// <summary>
    /// 流量數據(顯示所有流量主)
    /// </summary>
    public class ReferReportAllEntity
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
    
    
    /// <summary>
    /// 流量數據(顯示單一流量主)
    /// </summary>
    public class ReferReportSiteMasterEntity
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
      /// 日期
      /// </summary>
      public int date { get; set; }

    }
    
    /// <summary>
    /// 流量數據(顯示單一流量主底下所有網域數據)
    /// </summary>
    public class ReferReportSiteMasterDetailEntity
    {
        /// <summary>
        /// 流量主id
        /// </summary>
        public int account_id { get; set; }
        /// <summary>
        /// 網站名稱
        /// </summary>
        public string refer_url { get; set; }
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
        /// 日期
        /// </summary>
        public int date { get; set; }
    }
}