namespace MWM.API.Advert.Service.Application.VideoAdvert.video.Contract
{
    /// <summary>
    /// 新增跑馬燈
    /// </summary>
    public class MarqueeRequest
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
        /// 文字顏色
        /// </summary>
        public string color { get; set; }= string.Empty;

        /// <summary>
        /// 位置 0上,1下
        /// </summary>
        public int position { get; set; } = 0;
        /// <summary>
        /// 廣告文字
        /// </summary>
        public string advert_text { get; set; }= string.Empty;

        /// <summary>
        /// 循環方式 0:循環,1:出現一次
        /// </summary>
        public int loop_type { get; set; } = 0;

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
    /// 更新
    /// </summary>
    public class UpdateMarqueeRequest:MarqueeRequest
    {
        /// <summary>
        /// 唯一值
        /// </summary>
        public int id { get; set; }
    }

}