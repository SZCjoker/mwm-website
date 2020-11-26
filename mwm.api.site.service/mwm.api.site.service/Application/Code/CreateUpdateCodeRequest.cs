using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Code.Contract
{
    public class CreateUpdateCodeRequest
    {
        public Int32 id { get; set; } = 0;
        public Int32 account_id { get; set; } = 0;
        public String code_51la { get; set; } = null;
        public String code_cnzz { get; set; } = null;
        public String code_ga { get; set; } = null;
        public String code_ally { get; set; } = null;
        public Int16 state { get; set; }
    }
}
