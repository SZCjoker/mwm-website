using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable.Contract
{
    public class CreateRecordRequest
    {
        public Int64 id { get; set; }
        public Int64 account_id { get; set; }
        public Int64 bank_id { get; set; }
        public string ip { get; set; }
        public Decimal pay_amount { get; set; }
        public string password { get; set; }
        public Int32 cdate { get; set; }
        public Int32 ctime { get; set; }
    }
}
