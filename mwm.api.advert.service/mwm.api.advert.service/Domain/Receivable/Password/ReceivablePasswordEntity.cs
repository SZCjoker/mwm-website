using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.Receivable
{
    public class ReceivablePasswordEntity
    {
        public Int64 Id { get; set; }
        public Int64 AccountId { get; set; }
        public String Password { get; set; }
        public String LoginIp { get; set; }
        public Int32 Cdate { get; set; }
        public Int64 Ctime { get; set; }
        public Int32 Udate { get; set; }
        public Int64 Utime { get; set; }
        public Int16 FailCount { get; set; }
        public Int16 State { get; set; }
    }
}
