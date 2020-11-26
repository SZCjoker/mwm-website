using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable.Record
{
    public class QueryRequest
    {
        public string login_name { get; set; }
        public string begin_date { get; set; }
        public string end_date { get; set; }
        public Double fees { get; set; }
        public Int16 is_paid { get; set; }
        public int page_offset { get; set; }
        public int page_size { get; set; }
    }
}
