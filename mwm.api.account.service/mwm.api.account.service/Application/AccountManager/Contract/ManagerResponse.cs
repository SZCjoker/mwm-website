using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountManager.Contract
{
    public class ManagerResponse
    {
        public int id { get; set; }
        public string login_name { get; set; }
        public int role_id { get; set; }
        public string login_ip { get; set; }
        public int login_time { get; set; }
        public int ctime { get; set; }
        public int utime { get; set; }
        public int state { get; set; }
        public int login_fail_count { get; set; }
    }
}
