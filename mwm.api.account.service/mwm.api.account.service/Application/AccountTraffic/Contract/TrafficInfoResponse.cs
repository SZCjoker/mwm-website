using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountTraffic.Contract
{
    public struct TrafficInfoResponse
    {
        public int id { get; set; }
        public string login_name { get; set; }
        public string email { get; set; }
        public string cellphone { get; set; }
        public string secret_id { get; set; }
        public string secret_key { get; set; }
        public IEnumerable<ContactAccount> contact_account { get; set; }
    }
}
