using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountTraffic.Contract
{
    public struct LoginTrafficRequest
    {
        public string login_name { get; set; }
        public string password { get; set; }
        public string login_ip { get; set; }
    }
}
