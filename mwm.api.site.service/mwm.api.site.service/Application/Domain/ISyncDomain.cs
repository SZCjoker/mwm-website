using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Domain
{
public  interface ISyncDomain
    {
        Task<BasicResponse> SyncDomian(HttpMethod method, string sid, string Domain, int state = 0);
    }
}
