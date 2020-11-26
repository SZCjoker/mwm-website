using System;

namespace MWM.API.Account.Service.Application.Message.Contract
{
    public class CreateUpdateMessageRequest
    {
        public Int64 id { get; set; }
        /// <summary>
        /// 發訊者ID
        /// </summary>
        public Int32 account_id { get; set; }
        public string login_name { get; set; }
        /// <summary>
        /// 訊息主題Id
        /// </summary>
        public Int64 topic_id { get; set; } = 0;
        public String title { get; set; } = null;
        /// <summary>
        /// 訊息主體
        /// </summary>
        public String body { get; set; } = null;
        /// <summary>
        /// 訊息回復狀態  0 :提問,1:回應
        /// </summary>
        public Int16 type { get; set; } = 0;
        public Int32 cdate { get; set; }
        public Int32 ctime { get; set; } 
        public Int16 state { get; set; }
    }
}
