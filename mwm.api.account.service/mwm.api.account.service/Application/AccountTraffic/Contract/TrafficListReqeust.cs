using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountTraffic.Contract
{
    public class TrafficListRequest : PagingRequest
    {
        public int account_id { get; set; }
        public string login_name { get; set; }
        public int state { get; set; }
    }
}
