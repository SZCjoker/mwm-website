using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountTraffic.Contract
{
    public struct TrafficRequest
    {
        public String login_name { get; set; }
        public String email { get; set; }
        public String cellphone { get; set; }
        public String password { get; set; }
        public int state { get; set; }
        public string name { get; set; }
    }
}
