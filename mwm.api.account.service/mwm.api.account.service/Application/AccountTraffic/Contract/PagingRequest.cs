using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountTraffic.Contract
{
    public class PagingRequest
    {
        public int page { get; set; } = 0;
        public int size { get; set; } = 50;
        public string orderby { get; set; } = "id";
    }
}
