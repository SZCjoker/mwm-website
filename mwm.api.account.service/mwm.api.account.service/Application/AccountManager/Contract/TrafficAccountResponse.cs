using MWM.API.Account.Service.Application.AccountTraffic.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountManager.Contract
{
    public class TrafficAccountResponse
    {
        public Int32 id { get; set; }
        public String login_name { get; set; }
        public String email { get; set; }
        public String cellphone { get; set; }
        public String secret_key { get; set; }
        public String secret_id { get; set; }
        public Int64 ctime { get; set; }
        public Int64 utime { get; set; }
        public String login_ip { get; set; }
        public Int64 login_time { get; set; }
        public Int32 login_fail_count { get; set; }
        public string domain_name { get; set; }
        public int state { get; set; }
        public IEnumerable<ContactAccount> contact_account { get; set; }
    }
}
