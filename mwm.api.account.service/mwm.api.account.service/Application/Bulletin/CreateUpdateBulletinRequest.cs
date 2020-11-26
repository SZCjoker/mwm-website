using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Bulletin.Contract
{
    public class CreateUpdateBulletinRequest
    {
        public Int64 id { get; set; } = 0;           
        /// <summary>
        /// 置頂排序 1
        /// </summary>
        public Int32 sequence { get; set; } = 0;
        /// <summary>
        /// 公告標題
        /// </summary>
        public String title { get; set; } = null;
        /// <summary>
        /// 公告主體
        /// </summary>
        public String body { get; set; } = null;
        /// <summary>
        /// 附圖路徑
        /// </summary>
        public string pic_path { get; set; } = null;
        public Int32 cdate { get; set; }
        public Int32 ctime { get; set; }
        public Int32 udate { get; set; }
        public Int32 utime { get; set; }
        public Int16 state { get; set; }
    }
}
