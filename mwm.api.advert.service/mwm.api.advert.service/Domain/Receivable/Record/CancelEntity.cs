using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.Receivable.Record
{
    public class CancelEntity
    {
       public long Id { get; set; } 
       public long AccountId { get; set; } 
       public Int16 IsPaid { get; set; }
       public Int16 IsCancel { get; set; }
       public string CancelReason { get; set; }
       public Int32 Udate { get; set; }
       public Int64 Utime { get; set; }
    }
}
