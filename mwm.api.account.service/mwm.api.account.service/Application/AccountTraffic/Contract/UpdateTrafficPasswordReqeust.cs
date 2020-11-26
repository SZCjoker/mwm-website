using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountTraffic.Contract
{
    public struct UpdateTrafficPasswordRequest
    {
        public int account_id { get; set; }
        public string login_name { get; set; }
        public string password_new { get; set; }
        public string password_old { get; set; }
    }
}
