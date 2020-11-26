using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Common
{
    public struct BasicPagingResponse<T>
    { 
        public int code { get; set; }
        public string desc { get; set; }
        public T data { get; set; }
        public int total { get; set; }
    }
}
