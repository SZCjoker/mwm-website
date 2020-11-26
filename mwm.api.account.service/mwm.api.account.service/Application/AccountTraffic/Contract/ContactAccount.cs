using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountTraffic.Contract
{
    public class ContactAccount
    {
        public ContactAccountType account_type { get; set; }
        public string account_name { get; set; } = "";
    }

    public enum ContactAccountType
    {
        QQ = 1,
        WeChat = 2,
        Skype = 3
    }
}
