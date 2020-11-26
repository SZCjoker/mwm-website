using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Domain
{
    public class DomainEntity
    {
        public long Id { get; set; }
        public string DomainName { get; set; }
        public int Cdate { get; set; }
        public Int64 Ctime { get; set; }
        public int Udate { get; set; }
        public Int64 Utime { get; set; }
        public int State { get; set; }
    }
}
