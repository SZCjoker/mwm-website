using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Code
{
    public class CodeResponse
    {
        public Int64 ?id { get; set; }
        public Int64 ?account_id { get; set; }
        public String code_51la { get; set; }
        public String code_cnzz { get; set; }
        public String code_ga { get; set; }
        public String code_ally { get; set; }
        public Int32 ?cdate { get; set; }
        public Int64 ?ctime { get; set; }
        public Int32 ?udate { get; set; }
        public Int64 ?utime { get; set; }
        public Int16 ?state { get; set; }
    }
}
