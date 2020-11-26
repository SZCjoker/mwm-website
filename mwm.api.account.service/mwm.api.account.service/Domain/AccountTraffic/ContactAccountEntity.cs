using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.AccountTraffic
{
    public class ContactAccountEntity
    {
        public int account_id { get; set; }
        public string contact_account { get; set; }
        public int account_type { get; set; }
    }
}
