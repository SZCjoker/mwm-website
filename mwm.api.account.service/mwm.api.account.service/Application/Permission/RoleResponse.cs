using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Permission
{
    public class RoleResponse
    {
        public int id { get; set; }
        public string role_name { get; set; }
        public IEnumerable<int> code { get; set; }
    }
}
