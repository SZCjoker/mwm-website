using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable.Contract
{
    public class PasswordResponse
    {  
        public Int64 id { get; set; }
        public Int64 account_id { get; set; }
        public String password { get; set; }   
        public string login_ip { get; set; }
        public Int32 udate { get; set; }
        public Int64 utime { get; set; }
        public Int16 fail_count { get; set; }
        public Int16 state { get; set; }
    }
}
