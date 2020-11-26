using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.Receivable
{
    public class MerchantCashOutRecordEntity
    {
        public Int64 Id { get; set; }
        public Int64 AccountId { get; set; }
        public String AccountName { get; set; }        
        public String BankNumber { get; set; }
        public String BankName { get; set; }
        public String UserName { get; set; }
        public String BankAddress { get; set; }
        public Int32 Cdate { get; set; }
        public Int64 Ctime { get; set; }
        public Int32 PayDate { get; set; }
        public Int64 PayTime { get; set; }
        public Int64 PayAccountId { get; set; }
        public Decimal PayAmount { get; set; }
        public Int16 IsNotify { get; set; }
        public Int16 IsPaid { get; set; }
    }
}
