namespace MWM.API.Advert.Service.Application.DefaultAdvert.Contract
{
    /// <summary>
    /// 片頭彈窗廣告
    /// </summary>
    public class VideoPopUpResponse
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
        ///     狀態
        /// </summary>
        public int status { get; set; }

        /// <summary>
        ///     更新時間
        /// </summary>
        public long utime { get; set; }
    }
    
    /// <summary>
    /// 更新片頭彈窗廣告
    /// </summary>
    public class VideoPopUpUpdateRequest
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
        ///     狀態
        /// </summary>
        public int status { get; set; }

    }
}