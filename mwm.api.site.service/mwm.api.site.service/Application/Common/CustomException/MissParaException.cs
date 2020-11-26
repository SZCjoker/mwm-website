using MWM.API.Site.Service.Application.Common.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Common.CustomException
{
    public class MissParaException : Exception, IException
    {
        private readonly string _Msg
;
        public MissParaException()
        {
         
        }

        public MissParaException(string Msg)
        {
            _Msg = Msg;
        }

        public int StatusCode => (int)HttpStatusCode.BadRequest;

        public string Code => $"{this.StatusCode}";

        public string ErrorMessage =>$"{this._Msg}";

        public string Msg { get; set; }
    }
}
