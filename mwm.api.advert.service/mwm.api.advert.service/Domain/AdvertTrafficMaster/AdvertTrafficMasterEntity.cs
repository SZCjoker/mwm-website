namespace MWM.API.Advert.Service.Domain.AdvertTrafficMaster
{
    /// <summary>
    /// 流量主後台-我的廣告
    /// </summary>
    public class AdvertTrafficMasterEntity
    {
        /// <summary>
        ///  唯一值
        /// </summary>
        public int id { get; set; }
        /// <summary>
        ///  商戶ID
        /// </summary>
        public int account_id { get; set; }
        /// <summary>
        /// 0:漂浮廣告,1:全站彈跳廣告,2:片頭彈窗廣告,3:上方橫幅廣告,4:影片banner廣告
        /// </summary>
        public int default_ads_type { get; set; } 
        /// <summary>
        /// 廣告超連結
        /// </summary>
        public string hyper_link { get; set; }
        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string image_link { get; set; }

        /// <summary>
        /// 說明
        /// </summary>
        public string desc { get; set; } = string.Empty;


        /// <summary>
        /// 上方橫幅廣告排序
        /// </summary>
        public int banner_sort { get; set; } = 0;
        
        /// <summary>
        /// 廣告位置
        /// </summary>
        public string position { get; set; }= string.Empty;
        /// <summary>
        /// 建立時間
        /// </summary>
        public long ctime { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public long utime { get; set; }

        /// <summary>
        /// 0:停用,1:啟用
        /// </summary>
        public int state { get; set; } = 1;
    }
}