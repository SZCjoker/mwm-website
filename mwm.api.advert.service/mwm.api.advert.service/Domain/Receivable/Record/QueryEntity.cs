using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.Receivable.Record
{
    public class QueryEntity
    {
        public Int64 Id { get; set; }
        public string LoginName { get; set; }
        public string BankNumber { get; set;}
        public string BankName { get; set; }
        public string UserName { get; set; }
        public string Cdate { get; set; }
        public string Ctime { get; set; }
        public string PayDate { get; set; }
        public string PayTime { get; set; }
        public decimal PayAmount { get; set; }
        public Int64 PayAccountId { get; set; }
        public Double Fees { get; set; }
        public Int16 IsPaid { get; set; }
        public int PageOffset { get; set; }
        public int PageSize { get; set; }
    }
}
