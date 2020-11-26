namespace MWM.API.Advert.Service.Application.AdvertTrafficMaster.Contract
{
    /// <summary>
    ///     新增上方橫幅廣告Request
    /// </summary>
    public class AddTopBannerRequest
    {
        /// <summary>
        ///     廣告超連結
        /// </summary>
        public string hyper_link { get; set; }

        /// <summary>
        ///     圖片路徑
        /// </summary>
        public string image_link { get; set; }

        /// <summary>
        ///     備註
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 上方橫幅廣告排序
        /// </summary>
        public int banner_sort { get; set; }
    }

    /// <summary>
    ///     更新上方橫幅廣告Request
    /// </summary>
    public class UpdateTopBannerRequest
    {
        /// <summary>
        ///     唯一值
        /// </summary>
        public int id { get; set; }

        /// <summary>
        ///     廣告超連結
        /// </summary>
        public string hyper_link { get; set; }

        /// <summary>
        ///     圖片路徑
        /// </summary>
        public string image_link { get; set; }

        /// <summary>
        ///     備註
        /// </summary>
        public string desc { get; set; }
        
        /// <summary>
        /// 上方橫幅廣告排序
        /// </summary>
        public int banner_sort { get; set; }
    }

    /// <summary>
    ///     上方橫幅廣告Response
    /// </summary>
    public class TopBannerResponse
    {
        /// <summary>
        ///     唯一值
        /// </summary>
        public int id { get; set; }

        /// <summary>
        ///     廣告超連結
        /// </summary>
        public string hyper_link { get; set; }

        /// <summary>
        ///     圖片路徑
        /// </summary>
        public string image_link { get; set; }

        /// <summary>
        ///     備註
        /// </summary>
        public string desc { get; set; }

        /// <summary>
        ///     更新時間
        /// </summary>
        public long utime { get; set; }
        
        /// <summary>
        /// 上方橫幅廣告排序
        /// </summary>
        public int banner_sort { get; set; }
    }
}