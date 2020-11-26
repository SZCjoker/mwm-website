using Microsoft.IdentityModel.JsonWebTokens;
using MWM.API.Account.Service.Application.AccountManager.Contract;
using MWM.API.Account.Service.Domain.SyncDefaultAdvert;
using MWM.Extensions.Authentication.JWT;
using Phoenixnet.Extensions.Caching;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Phoenixnet.Extensions.Method;
using MWM.API.Account.Service.Domain.AccountManager;
using MWM.API.Account.Service.Application.AccountTraffic;
using MWM.API.Account.Service.Application.AccountTraffic.Contract;

namespace MWM.API.Account.Service.Application.AccountManager
{
    public class AccountManagerService : IAccountManagerService
    {
        private readonly IGenerateJwtTokenService _jwt;
        private readonly IAccountManagerRepository _repository;
        private readonly IAccountTrafficService _trafficService;
        private readonly ISyncDefaultAdvert _syncDefault;
        private readonly ICachingService _caching;

        public AccountManagerService(IGenerateJwtTokenService jwt, IAccountManagerRepository repository, IAccountTrafficService trafficService, ISyncDefaultAdvert syncDefaultAdvert, ICachingService caching)
        {
            _jwt = jwt;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _trafficService = trafficService;
            _syncDefault = syncDefaultAdvert;
            _caching = caching;
            //_sync = sync;
        }

        public async ValueTask<BasicResponse<LoginManagerResponse>> LoginAsync(LoginManagerRequest request)
        {
            var entity = await _repository.GetByName(request.login_name);
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (entity.login_name == null)
                return new BasicResponse<LoginManagerResponse>() { desc = "登入失敗", code = 399 };

            if (entity.state != 1)
                return new BasicResponse<LoginManagerResponse>() { desc = "無效帳號", code = 397 };

            if (entity.password != request.password.ToMD5())
                return new BasicResponse<LoginManagerResponse>() { desc = "登入失敗", code = 399 };

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sid, entity.id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, request.login_name),
                new Claim(ClaimTypes.Surname, "")
            };

            var token = _jwt.GenerateJwtToken(claims, null);

            return new BasicResponse<LoginManagerResponse>()
            {
                desc = "success",
                code = 200,
                data = new LoginManagerResponse()
                {
                    login_name = request.login_name,
                    token = token,
                    account_id = entity.id,
                    role_id = entity.role_id
                }
            };
        }

        public async ValueTask<BasicResponse<ManagerResponse>> GetManagerInfoAsync(int id)
        {
            var entity = await _repository.Get(id);
            var response = new ManagerResponse()
            {
                id = entity.id,
                login_name = entity.login_name,
                role_id = entity.role_id,
                login_ip = entity.login_ip,
                login_time = entity.login_time,
                ctime = entity.ctime,
                utime = entity.utime,
                state = entity.state,
                login_fail_count = entity.login_fail_count
            };

            return StateCodeHandler.ForRecord(response);
        }

        public async ValueTask<PagingResponse<IEnumerable<ManagerResponse>>> GetManagerAllAsync(ManagerListRequest request)
        {
            var paging = new Paging(request.page, request.size);
            var listEntity = new ListEntity()
            {
                login_name = $"%{request.login_name}%" ?? string.Empty,
                PageOffset = paging.Offset,
                PageSize = request.size,
                Order = request.orderby
            };
            var (entities, total) = await _repository.GetAll(listEntity);
            paging.RowsTotal = total;

            var response = entities.Select(entity => new ManagerResponse()
            {
                id = entity.id,
                login_name = entity.login_name,
                role_id = entity.role_id,
                login_ip = entity.login_ip,
                login_time = entity.login_time,
                ctime = entity.ctime,
                utime = entity.utime,
                state = entity.state,
                login_fail_count = entity.login_fail_count
            });

            return StateCodeHandler.ForPagingCount<IEnumerable<ManagerResponse>>((int)total, response, paging);
        }


        public async ValueTask<BasicResponse<int>> CreateAsync(ManagerRequest request)
        {
            var data = await _repository.GetByName(request.login_name);
            if (data != null)
                return new BasicResponse<int>() { desc = "帳號重複", code = 301, data = 0 };

            var time = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var entity = new AccountManagerEntity()
            {
                id = await _trafficService.GenerateId(),
                login_name = request.login_name,
                password = request.password,
                role_id = request.role_id,
                ctime = time,
                utime = time,
                login_ip = "0.0.0.0",
                state = 1
            };

            var count = await _repository.Create(entity);

            if (count.Equals(0))
                return StateCodeHandler.ForCount(count, 0);

            return StateCodeHandler.ForCount<int>(count, entity.id);
        }

        public async ValueTask<BasicResponse> UpdateAsync(ManagerRequest request)
        {
            var data = await _repository.Get(request.account_id);
            if (data == null)
                return StateCodeHandler.ForBool(false);

            var time = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var entity = new AccountManagerEntity()
            {
                id = request.account_id,
                login_name = request.login_name,
                password = request.password,
                role_id = request.role_id,
                utime = time
            };

            var count = await _repository.Update(entity);

            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse> ChangeStateAsync(int id, int state)
        {
            var data = await _repository.Get(id);

            if (data.login_name == null)
                return StateCodeHandler.ForBool(false);

            var time = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var count = await _repository.ChangeAccountState(id, state, time);

            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<PagingResponse<IEnumerable<TrafficAccountResponse>>> GetTrafficAccountPageAsync(string loginName, int state, int page, int size)
        {
            var paging = new Paging(page, size);
            loginName = loginName == null ? $" '' " : $" '%{loginName}%' ";
            var instance = await _repository.GetAccountTraffic(loginName, state, paging.Offset, size);
            var entities = instance.rows;
            paging.RowsTotal = instance.total;

            var result = entities.Select(entity => new TrafficAccountResponse()
            {
                id = entity.id,
                login_name = entity.login_name,
                email = entity.email,
                cellphone = entity.cellphone,
                secret_id = entity.secret_id,
                secret_key = entity.secret_key,
                login_time = entity.login_time,
                login_ip = entity.login_ip,
                ctime = entity.ctime,
                utime = entity.utime,
                login_fail_count = entity.login_fail_count,
                domain_name = entity.domain_name,
                state = entity.state,
                contact_account = SpiltString(entity.contact_account, entity.account_type)
            });

            return StateCodeHandler.ForPagingCount((int)paging.RowsTotal, result, paging);
        }

        private IEnumerable<ContactAccount> SpiltString(string contact_account, string account_type)
        {
            if (contact_account != null)
            {
                var contactAccount = contact_account.Split(',');
                var accountType = account_type.Split(',').Select(int.Parse).ToArray();
                for (int i = 0; i < accountType.Count(); i++)
                {
                    yield return new ContactAccount()
                    {
                        account_name = contactAccount[i],
                        account_type = (ContactAccountType)accountType[i]
                    };
                }
            }

            //yield return new ContactAccount();

        }
    }
}
