namespace MWM.API.Advert.Service.Application.VideoAdvert.video.Contract
{
    /// <summary>
    /// 影片廣告回傳參數
    /// </summary>
    public class VideoAllResponse
    {
        /// <summary>
        /// 唯一值
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 廣告名稱
        /// </summary>
        public string title { get; set; } 
        
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
        public int status { get; set; } 
        /// <summary>
        /// 備註
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 該筆資料詳細資訊
        /// </summary>
        public object  detail   { get; set; }
    }
}