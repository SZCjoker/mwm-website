using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Link
{
    public class CreateUpdateLinkRequest
    {
        public Int64 id { get; set; }
        public Int32 sequence { get; set; }
        public Int32 account_id { get; set; }
        public String name { get; set; }
        public String link { get; set; }
        public String desc { get; set; }
        public Int16 state { get; set; }
    }
}
