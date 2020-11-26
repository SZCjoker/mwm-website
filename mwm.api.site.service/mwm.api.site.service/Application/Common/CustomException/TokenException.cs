using MWM.API.Site.Service.Application.Common.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Common.CustomException
{
    public class TokenException : Exception, IException
    {
        public int StatusCode => (int)HttpStatusCode.Unauthorized;

        public string Code => $"{this.StatusCode}";

        public string ErrorMessage => $"UnAuthorized";
    }
}
