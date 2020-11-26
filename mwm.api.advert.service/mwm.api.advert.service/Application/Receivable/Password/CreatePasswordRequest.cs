using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable.Contract
{
    public class CreatePasswordRequest
    {
        public Int64 account_id { get; set; }
        public String password { get; set; }   
        public String ip { get; set; }
    }
}
