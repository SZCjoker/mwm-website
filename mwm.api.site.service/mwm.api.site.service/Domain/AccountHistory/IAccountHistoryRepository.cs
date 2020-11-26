using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.AccountHistory
{
 public interface IAccountHistoryRepository
    {
        Task<(IEnumerable<AccountHistoryEntity> rows, long total)> GetAccountHistoryByCondition(QueryAllEntity entity, string queryStr);

        Task<IEnumerable<AccountHistoryEntity>> GetAllHistory();

        Task<int> SaveAccountHistory(AccountHistoryEntity record);
    }
}
