using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable.Contract
{
    public class MerchantApplyResponse
    {
        public String account { get; set; }
        public String bank_number { get; set; }
        public String bank_name { get; set; }
        public String bank_user { get; set; }
        public String bank_address { get; set; }
        public Int64 apply_time { get; set; }
        public Decimal pay_amount { get; set; }
        public Int16 is_paid { get; set; }
    }
}
