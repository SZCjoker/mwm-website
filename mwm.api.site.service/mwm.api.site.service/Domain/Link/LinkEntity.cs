using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Link
{
    public class LinkEntity
    {
        public Int64 Id { get; set; }
        public Int32 Sequence { get; set; }
        public Int32 AccountId { get; set; }
        public String Name { get; set; }
        public String Link { get; set; }
        public String Desc { get; set; }
        public Int32 Cdate { get; set; }
        public Int64 Ctime { get; set; }
        public Int32 Udate { get; set; }
        public Int64 Utime { get; set; }
        public Int16 State { get; set; }
    }
}
