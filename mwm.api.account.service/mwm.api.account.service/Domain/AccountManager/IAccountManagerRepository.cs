using MWM.API.Account.Service.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.AccountManager
{
    public interface IAccountManagerRepository
    {
        ValueTask<int> ChangeAccountState(int id, int state, int time);
        ValueTask<int> CheckId(int id);
        ValueTask<int> Create(AccountManagerEntity entity);
        ValueTask<ManageAccountEntity> Get(int id);
        ValueTask<(IEnumerable<AccountTrafficEntity> rows, long total)> GetAccountTraffic(string loginName, int state, int page, int size);
        ValueTask<(IEnumerable<ManageAccountEntity> rows, long total)> GetAll(ListEntity entity);
        ValueTask<Account.ManageAccountEntity> GetByName(string login_name);
        ValueTask<int> Update(AccountManagerEntity entity);
    }
}
