using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Permission
{
    public class RoleFuntionRequest
    {
        public int role_id { get; set; }
        public IEnumerable<int> code { get; set; }
    }
}
