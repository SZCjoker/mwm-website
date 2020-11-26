using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.AccountManager
{
    public struct ListEntity
    {
        public string login_name { get; set; }
        public int PageOffset { get; set; }
        public int PageSize { get; set; }
        public string Order { get; set; }
    }
}

