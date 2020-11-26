using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountTraffic.Contract
{
    public class ResetPasswordRequest
    {
        public string token { get; set; }
        public string password { get; set; }
    }
}
