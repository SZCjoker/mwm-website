using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Code
{
    public class CodeEntity
    {
        public Int64 Id { get; set; }
        public Int32 AccountId { get; set; }
        public String Code51la { get; set; }
        public String CodeCnzz { get; set; }
        public String CodeGA { get; set; }
        public String CodeAlly { get; set; } 
        public Int32 Cdate { get; set; }
        public Int64 Ctime { get; set; }
        public Int32 Udate { get; set; }
        public Int64 Utime { get; set; }
        public Int16 State { get; set; }
    }
}