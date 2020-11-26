using MWM.API.Advert.Service.Application.Common.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Common.CustomException
{
    public class MissParaException:Exception,IException
    {


        public MissParaException()
        {

        }

        public MissParaException(string Msg)
        {
            ErrorMessage = Msg;
        }

        public int StatusCode => (int)HttpStatusCode.BadRequest;

        public string Code => $"{this.StatusCode}";

        public string ErrorMessage { get; set; }

       







    }
}
