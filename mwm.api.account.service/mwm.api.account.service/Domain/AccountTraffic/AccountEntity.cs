using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.Account
{
    public class AccountEntity
    {
        public Int32 Id { get; set; }
        public string LoginName { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public string Password { get; set; }
        public string SecretId { get; set; }
        public string SecretKey { get; set; }
        public Int32 CreateDate { get; set; }
        public Int64 CreateTime { get; set; }
        public Int32 UpdateDate { get; set; }
        public Int64 UpdateTime { get; set; }
        public string LoginIp { get; set; }
        public Int64 LoginTime { get; set; }
        public int State { get; set; }
        public Int32 LoginFailCount { get; set; }
        public string Name { get; set; }
        public string DomainName { get; set; } = "";
    }
}
