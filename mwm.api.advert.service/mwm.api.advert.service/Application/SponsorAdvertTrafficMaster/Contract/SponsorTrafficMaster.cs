namespace MWM.API.Advert.Service.Application.SponsorAdvertTrafficMaster.Contract
{
    /// <summary>
    /// 全民贊助傳入參數
    /// </summary>
    public class SponsorTrafficMasterRequest
    {
        
        /// <summary>
        /// 圖片連結
        /// </summary>
        public string image_link { get; set; }

        /// <summary>
        /// 網址連結
        /// </summary>
        public string hyper_link { get; set; }
        
        /// <summary>
        /// 裝置類型:0.PC,1.Mobile
        /// </summary>
        public int device_type { get; set; }

        /// <summary>
        /// 位置：0.頂天,1.地板
        /// </summary>
        public int position_type { get; set; }

        /// <summary>
        /// 說明
        /// </summary>
        public string desc { get; set; }


    }

    /// <summary>
    /// 輸出
    /// </summary>
    public class SponsorTrafficMasterResponse
    {
        /// <summary>
        /// 流水號-唯一值
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 流量主id
        /// </summary>
        public int account_id { get; set; }
        /// <summary>
        /// 圖片連結
        /// </summary>
        public string image_link { get; set; }

        /// <summary>
        /// 網址連結
        /// </summary>
        public string hyper_link { get; set; }

        /// <summary>
        /// 位置：0.頂天,1.地板
        /// </summary>
        public int position_type { get; set; }
        /// <summary>
        /// 裝置類型
        /// </summary>
        public int device_type { get; set; }

        /// <summary>
        /// 說明
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public long utime { get; set; }
    }

    
    
    
    
    
    
    /// <summary>
    /// 更新平面廣告-傳入參數
    /// </summary>
    public class UpdateSponsorTrafficMasterRequest : SponsorTrafficMasterRequest
    {
        public int id { get; set; }
    }
}