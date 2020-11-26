using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable.Contract
{
    public class UpdatePasswordRequest
    {
        public Int64 id { get; set; }
        public Int64 account_id { get; set; } = 0;
        public String password { get; set; } = null;
        public string newpassword { get; set; } = null;
        public string ip { get; set; } = null;
        public Int16 state { get; set; } =0;
    }
}
