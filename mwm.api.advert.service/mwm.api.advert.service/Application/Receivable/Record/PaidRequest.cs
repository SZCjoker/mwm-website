using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable.Record
{
    public class PaidRequest
    {
        public Int64 id { get; set; }
        public Int64 account_id {get;set;}
        public Int64 pay_account_id { get; set; }
        public decimal pay_amount { get; set; }
        public Int16  is_paid { get; set; }
    }
}
