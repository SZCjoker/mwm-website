using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.AccountHistory
{
    public class AccountHistoryEntity
    {
        public Int64 Id { get; set; }
        public string Loginname { get; set; }
        public Int64 AccountId { get; set; }
        public string Category { get; set; }
        public string Action { get; set; }
        public string UpdateData { get; set; }
        public string BeforeData { get; set; }
        public string AfterData { get; set; }
        public string Ip { get; set; }
        public int Udate { get; set; }
        public Int64 Utime { get; set; }
        public int Count { get; set; }
    }
}
