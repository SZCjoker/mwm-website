﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Message.Contract
{
    public class MessageResponse
    {
        public Int64 id { get; set; }
        public string login_name { get; set; }
        public Int64 account_id { get; set; }
        public Int64 topic_id { get; set; }
        public String title { get; set; }
        public String body { get; set; }
        public Int16 type { get; set; }
        public Int32 cdate { get; set; }
        public Int64 ctime { get; set; }
        public Int32 udate { get; set; }
        public Int64 utime { get; set; }
        public Int16 state { get; set; }
        public Int32 total { get; set; }
    }
}
