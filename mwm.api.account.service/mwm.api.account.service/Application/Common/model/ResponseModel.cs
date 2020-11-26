using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Common.model
{
    public class ResponseModel
    {
        public static Dictionary<string, string> ResMessage => new Dictionary<string, string>
        {
            { ok,"success"},
            { TokenException,"Unauthorized"},
        };

        public static string ok => "0000";

        public static string TokenException => "401";
    }
}
