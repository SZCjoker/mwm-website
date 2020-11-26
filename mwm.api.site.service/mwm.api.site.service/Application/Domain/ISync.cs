using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Domain
{
 public interface ISync
    {
        Task<HttpResponseMessage> SyncAsync(HttpMethod method, string url, string skey, string domain, int state);
    }
}
