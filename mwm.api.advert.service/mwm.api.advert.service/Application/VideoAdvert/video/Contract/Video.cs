namespace MWM.API.Advert.Service.Application.VideoAdvert.video.Contract
{
    /// <summary>
    /// 影片廣告參數
    /// </summary>
    public class VideoRequest
    {
        /// <summary>
        /// 廣告名稱
        /// </summary>
        public string title { get; set; } = string.Empty;
        /// <summary>
        /// 廣告連結
        /// </summary>
        public string hyper_link { get; set; }= string.Empty;
       
        /// <summary>
        /// 影片廣告連結
        /// </summary>
        public string video_link { get; set; }= string.Empty;
        /// <summary>
        /// 廣告可略過時間
        /// </summary>
        public int skip_time { get; set; }

        /// <summary>
        /// 預設時間 0:隨機出現
        /// </summary>
        public int default_time { get; set; } = 0;

        /// <summary>
        /// 開始時間
        /// </summary>
        public long start_time { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public long end_time { get; set; }

        /// <summary>
        /// 狀態.0關閉,1啟用
        /// </summary>
        public int status { get; set; } = 1;
 
        /// <summary>
        /// 備註
        /// </summary>
        public string desc { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class UpdateVideoRequest:VideoRequest
    {
        /// <summary>
        /// 唯一值
        /// </summary>
        public int id { get; set; }
    }
}