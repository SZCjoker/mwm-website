namespace MWM.API.Advert.Service.Application.DefaultAdvert.Contract
{
    /// <summary>
    ///     更新影片banner廣告告Request
    /// </summary>
    public class UpdateVideoBannerRequest
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
    }
    /// <summary>
    /// 新增影片banner廣告
    /// </summary>
    public class AddVideoBannerResponse
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
    }
    /// <summary>
    /// 影片廣告輸出
    /// </summary>
    public class AddVideoBannerRequest
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
    }
}