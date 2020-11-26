namespace MWM.API.Advert.Service.Domain.VideoAdvert.video
{
    /// <summary>
    /// 聯盟廣告-影片廣告
    /// </summary>
    public class VideoAdvertEntity
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 廣告名稱
        /// </summary>
        public string title { get; set; } = string.Empty;
        /// <summary>
        /// 廣告連結
        /// </summary>
        public string hyper_link { get; set; }= string.Empty;
        /// <summary>
        /// 文字顏色
        /// </summary>
        public string color { get; set; }= string.Empty;

        /// <summary>
        /// 位置
        /// </summary>
        public int position { get; set; } = 0;
        /// <summary>
        /// 廣告文字
        /// </summary>
        public string advert_text { get; set; }= string.Empty;

        /// <summary>
        /// 循環方式
        /// </summary>
        public int loop_type { get; set; } = 0;

        /// <summary>
        /// 預設時間
        /// </summary>
        public int default_time { get; set; } = 0;
        /// <summary>
        /// 影片廣告連結
        /// </summary>
        public string video_link { get; set; }= string.Empty;

        /// <summary>
        /// 廣告可略過時間
        /// </summary>
        public int skip_time { get; set; } = 0;
        /// <summary>
        /// 開始時間
        /// </summary>
        public long start_time { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public long end_time { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public int status { get; set; } = 1;
        /// <summary>
        /// 影片廣告類型.0跑馬燈,1影片廣告
        /// </summary>
        public int video_advert_type { get; set; } 
        /// <summary>
        /// 備註
        /// </summary>
        public string desc { get; set; }
        
        /// <summary>
        /// 建立時間
        /// </summary>
        public long ctime { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public long utime { get; set; }
    }


}