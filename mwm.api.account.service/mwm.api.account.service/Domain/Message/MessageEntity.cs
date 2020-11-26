using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.Message
{
    public class MessageEntity
    {
        public Int64 Id { get; set; }
        public Int64 AccountId { get; set; }
        public string LoginName { get; set; }
        public Int64 TopicId { get; set; }
        public String Title { get; set; }
        public String Body { get; set; }
        public Int16 Type { get; set; }
        public Int32 Cdate { get; set; }
        public Int64 Ctime { get; set; }
        public Int32 Udate { get; set; }
        public Int64 Utime { get; set; }
        public Int16 State { get; set; }
        public Int32 Total { get; set; }
    }
}
