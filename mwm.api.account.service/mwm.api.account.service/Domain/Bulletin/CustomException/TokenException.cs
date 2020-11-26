using MWM.API.Account.Service.Application.Common.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.Bulletin.CustomException
{
    public class TokenException : Exception, IException
    {
        public int StatusCode => (int)HttpStatusCode.Unauthorized;

        public string Code => ResponseModel.TokenException;

        public string ErrorMessage => ResponseModel.ResMessage[Code];
    }
}
