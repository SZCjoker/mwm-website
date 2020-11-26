using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountTraffic.Contract
{
    public class LoginTrafficResponse
    {
        public string login_name { get; set; }
        public string token { get; set; }
        public int account_id { get; set; }
    }
}
