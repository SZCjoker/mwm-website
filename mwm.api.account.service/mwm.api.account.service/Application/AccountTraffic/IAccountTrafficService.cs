using MWM.API.Account.Service.Application.AccountTraffic.Contract;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.AccountTraffic
{
    public interface IAccountTrafficService
    {
        Task<BasicResponse<int>> CreateAsync(TrafficRequest request);
        //Task<BasicResponse> DeleteAsync(int accountId);
        //Task<BasicResponse> EnableAccountAsync(int accountId, int parentAccountId);
        Task<int> GenerateId();
        Task<PagingResponse<IEnumerable<TrafficResponse>>> GetAllTrafficAsync(TrafficListRequest request);
        Task<BasicResponse<TrafficInfoResponse>> GetInfoAsync(int accountId);
        //Task<PagingResponse<IEnumerable<AccountResponse>>> GetNextLevelAsync(AccountListRequest request);
        Task<BasicResponse<LoginTrafficResponse>> LoginAsync(LoginTrafficRequest request);
        ValueTask<BasicResponse> ForgetPasswordAsync(string account, string mail, string domain);
        Task<BasicResponse> UpdateAsync(TrafficUpdateRequest request);
        Task<BasicResponse> UpdatePasswordAsync(UpdateTrafficPasswordRequest request);
        ValueTask<BasicResponse> ResetPasswordAsync(int account_id);
        ValueTask<BasicResponse> ForgetPasswordResetAsync(ResetPasswordRequest request);
    }
}
