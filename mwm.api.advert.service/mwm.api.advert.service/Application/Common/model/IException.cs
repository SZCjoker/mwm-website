﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Common.model
{
 public   interface IException
    {
        int StatusCode { get; }
        string Code { get; }
        string ErrorMessage { get; set; }
    }
}
