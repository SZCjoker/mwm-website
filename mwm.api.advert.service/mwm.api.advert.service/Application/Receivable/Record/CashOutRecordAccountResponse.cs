using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable.Contract
{
    public class CashOutRecordAccountResponse
    {
        public Int64 id { get; set; }
        public Int64 account_id { get; set; }
        public string account_name { get; set; }
        public String bank_number { get; set; }
        public String bank_name { get; set; }
        public string bank_address { get; set; }
        public string user_name { get; set; }
        public string apply_time { get; set; }
        public string pay_time { get; set; }
        public Decimal pay_amount { get; set; }
        public Int16 is_notify { get; set; }
        public Int16 is_paid { get; set; }
    }
}
