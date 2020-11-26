using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Template
{
    public class TemplateResponse
    {  public Int64 id { get; set; }
        public string img { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public int advert_amount { get; set; }
        public int cdate { get; set; }
        public Int64 ctime { get; set; }
        public int udate { get; set; }
        public Int64 utime { get; set; }
        public int state { get; set; }
    }
}
