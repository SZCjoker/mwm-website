using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.Permission
{
    public class RoleFunctionMapping
    {
        public int role_id { get; set; }
        public int function_id { get; set; }
        public int ctime { get; set; }
        public int utime { get; set; }
        public int state { get; set; }
    }
}
