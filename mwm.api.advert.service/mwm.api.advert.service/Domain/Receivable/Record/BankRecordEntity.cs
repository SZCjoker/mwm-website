using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.Receivable
{
    public class BankRecordEntity
    {
        public Decimal PayAmount { get; set; }
        public string AccountName { get; set; }
        public String BankNumber { get; set; }
        public String BankName { get; set; }
        public string UserName { get; set; }
        public String BankAddress { get; set; }
        public Int16 IsPaid { get; set; }
    }
}
