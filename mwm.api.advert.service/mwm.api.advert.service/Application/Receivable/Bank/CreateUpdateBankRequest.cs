using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable.Contract
{
    public class CreateUpdateBankRequest
    {
        public Int64 id { get; set; }
        public Int64 account_id { get; set; }
        public String bank_name { get; set; } = null;
        /// <summary>
        /// 卡號
        /// </summary>
        public String bank_number { get; set; } = null;
        /// <summary>
        /// 戶名
        /// </summary>
        public String user_name { get; set; } = null;
        public String city { get; set; }
        public String province { get; set; }
        public String address { get; set; }
        public Int16 state { get; set; }
    }
}
