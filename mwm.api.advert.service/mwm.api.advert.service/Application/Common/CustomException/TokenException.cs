using MWM.API.Advert.Service.Application.Common.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Common.CustomException
{
    public class TokenException : Exception, IException
    {

      

        public TokenException()
        {
            ErrorMessage = $"without token";
        }
        public TokenException(string CustomErrorMessage)
        {
            ErrorMessage = CustomErrorMessage;
        }


        public int StatusCode => (int)HttpStatusCode.Unauthorized;

        public string Code => $"{this.StatusCode}";

        public string ErrorMessage { get; set; }
    }
}
