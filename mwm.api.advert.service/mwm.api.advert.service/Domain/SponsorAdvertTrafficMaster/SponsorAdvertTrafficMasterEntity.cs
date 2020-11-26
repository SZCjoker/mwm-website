namespace MWM.API.Advert.Service.Domain.SponsorAdvertTrafficMaster
{
    /// <summary>
    /// 流量主-全民贊助廣告
    /// </summary>
    public class SponsorAdvertTrafficMasterEntity
    {    
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int account_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string image_link { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string hyper_link { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int device_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int position_type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string desc { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public long ctime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long utime { get; set; }
    }
}