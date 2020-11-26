using MWM.API.Account.Service.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.AccountTraffic
{
    public interface IAccountRepository
    {
        Task<int> ChangeAccountState(int accountId, int state);
        Task<int> CheckId(int id);
        Task<int> Create(AccountEntity entity);
        Task<AccountEntity> Get(int accountId);
        Task<(IEnumerable<AccountEntity> rows, long total)> GetTrafficAll(ListEntity entity);
        Task<AccountEntity> GetByName(string account);
        Task<IEnumerable<string>> GetNextLevelAccountName(int parentId, int isMerchant, int state, string filter = "");
        ValueTask<int> ResetPassword(int accountId, string defaultPassword);
        Task<int> Update(AccountEntity entity);
        Task<int> UpdateLoginFailCount(int id, long timeStamp, string ip, int failCount = 0);
        Task<IEnumerable<ContactAccountEntity>> GetContactAccount(int accountId);
        Task<int> UpdateContactAccount(int account_id, string insertSql);
    }
}
