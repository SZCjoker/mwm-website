using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable.Record
{
    public class CancelRequest
    {
        public long id { get; set; }
        public long account_id { get; set; }
        public Int16 is_paid { get; set; }
        public Int16 is_cancel { get; set; }
        public string cancel_reason { get; set; }
    }
}
