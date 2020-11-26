using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountManager.Contract
{
    public class LoginManagerRequest
    {
        public string login_name { get; set; }
        public string password { get; set; }
        public string login_ip { get; set; }
    }
}
