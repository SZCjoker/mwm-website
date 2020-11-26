using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Template
{
    public class TemplateEntity
    {
        public Int64 Id { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int AdvertAmount { get; set; }
        public int Cdate { get; set; }
        public Int64 Ctime { get; set; }
        public int Udate { get; set;}
        public Int64 Utime { get; set; }
        public int State { get; set; }
    }
}
