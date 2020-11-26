using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Message
{
    public class QueryCondition
    {   
        /// <summary>
        /// 訊息回復狀態
        /// </summary>
        public short? state { get; set; }
        public string login_name { get; set; }
        public String title { get; set; }
        public int pageoffset { get; set; }
        public int pagesize { get; set; }
    }
}
