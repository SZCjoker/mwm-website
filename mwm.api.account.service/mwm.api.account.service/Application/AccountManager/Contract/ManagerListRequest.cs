using MWM.API.Account.Service.Application.AccountTraffic.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountManager.Contract
{
    public class ManagerListRequest : PagingRequest
    {
        public string login_name { get; set; }
    }
}
