using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.AccountManager
{
    public class AccountTrafficEntity
    {
        public int id { get; set; }
        public string login_name { get; set; }
        public string email { get; set; }
        public string cellphone { get; set; }
        public string secret_id { get; set; }
        public string secret_key { get; set; }
        public string login_ip { get; set; }
        public int login_time { get; set; }
        public int ctime { get; set; }
        public int utime { get; set; }
        public int state { get; set; }
        public int login_fail_count { get; set; }
        public string domain_name { get; set; }
        public string contact_account { get; set; }
        public string account_type { get; set; }
    }
}
