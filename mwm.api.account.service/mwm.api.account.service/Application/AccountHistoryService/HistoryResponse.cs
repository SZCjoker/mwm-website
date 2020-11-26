using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountHistoryService
{
    public class HistoryResponse
    {
        public string login_name { get; set; }
        public Int64 account_id { get; set; }
        public string action { get; set; }
        public string category { get; set; }
        public string update_data { get; set; }
        public string before_data { get; set; }
        public string after_data { get; set; }
        public string ip { get; set; }
        public int udate { get; set; }
        public Int64 utime { get; set; }
        public int count { get; set; }
    }
}
