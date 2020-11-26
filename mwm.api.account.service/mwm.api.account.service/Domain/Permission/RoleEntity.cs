using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.Permission
{
    public class RoleEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public int ctime { get; set; }
        public int utime { get; set; }
        public int state { get; set; }
    }
}
