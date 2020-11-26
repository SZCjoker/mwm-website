using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.AccountHistory
{
 public interface IAccountHistoryService
    {
        Task<PagingResponse<IEnumerable<HistoryResponse>>> GetAccountHistoryByCondition(QueryRequest request);

        Task<BasicResponse<IEnumerable<HistoryResponse>>> GetAllHistory();

        Task<BasicResponse> SaveAccountHistory(HistoryRequest entity);
    }
}
