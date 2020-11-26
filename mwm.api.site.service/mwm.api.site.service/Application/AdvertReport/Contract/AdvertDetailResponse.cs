namespace MWM.API.Site.Service.Application.AdvertReport.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class AdvertDetailResponse
    {
        /// <summary>
        /// 商戶id
        /// </summary>
        public int account_id { get; set; }
        /// <summary>
        /// 網域名稱
        /// </summary>
        public string referer { get; set; }
        /// <summary>
        /// pc點擊數
        /// </summary>
        public int pc_clicks { get; set; }
        /// <summary>
        /// pc觀看數
        /// </summary>
        public int pc_views { get; set; }
        /// <summary>
        /// 行動裝置觀看數
        /// </summary>
        public int mobile_views { get; set; }
        /// <summary>
        /// 行動裝置點擊數
        /// </summary>
        public int mobile_clicks { get; set; }
    }
}