using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountTraffic.Contract
{
    public class TrafficUpdateRequest
    {
        public int account_id { get; set; }
        public string email { get; set; } = "";
        public string cellphone { get; set; } = "";
        public string password { get; set; } = "";
        public string name { get; set; } = "";
        public IEnumerable<ContactAccount> contact_account { get; set; }
        public int state { get; set; }
    }
}
