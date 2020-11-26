using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountManager.Contract
{
    public class LoginManagerResponse
    {
        public string login_name { get; set; }
        public string token { get; set; }
        public int account_id { get; set; }
        public int role_id { get; set; }
    }
}
