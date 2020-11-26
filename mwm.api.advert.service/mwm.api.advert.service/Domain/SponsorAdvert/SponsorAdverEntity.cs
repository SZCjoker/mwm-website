namespace MWM.API.Advert.Service.Domain.SponsorAdvert
{
    /// <summary>
    /// 
    /// </summary>
    public class SponsorAdverEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }

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