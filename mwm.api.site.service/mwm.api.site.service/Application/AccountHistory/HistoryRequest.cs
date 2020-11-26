using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.AccountHistory
{
    public class HistoryRequest
    {
        public Int64 account_id { get; set; }
        public string category { get; set; }
        public string control_name { get; set; }
        public string restful_method { get; set; }
        public string before_data { get; set; }
        public string update_data { get; set; }
    }
}
