using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.Account
{
    public struct ListEntity
    {
        public int AccountId { get; set; }
        public string LoginName { get; set; }
        public int State { get; set; }
        public int PageOffset { get; set; }
        public int PageSize { get; set; }
        public string Order { get; set; }
    }
}
