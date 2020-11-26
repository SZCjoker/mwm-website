using MWM.API.Account.Service.Application.AccountManager.Contract;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountManager
{
    public interface IAccountManagerService
    {
        ValueTask<BasicResponse> ChangeStateAsync(int id, int state);
        ValueTask<BasicResponse<int>> CreateAsync(ManagerRequest request);
        ValueTask<PagingResponse<IEnumerable<ManagerResponse>>> GetManagerAllAsync(ManagerListRequest request);
        ValueTask<BasicResponse<ManagerResponse>> GetManagerInfoAsync(int id);
        ValueTask<PagingResponse<IEnumerable<TrafficAccountResponse>>> GetTrafficAccountPageAsync(string loginName, int state, int page, int size);
        ValueTask<BasicResponse<LoginManagerResponse>> LoginAsync(LoginManagerRequest request);
        ValueTask<BasicResponse> UpdateAsync(ManagerRequest request);
    }
}
