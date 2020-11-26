using Microsoft.IdentityModel.JsonWebTokens;
using MWM.API.Account.Service.Domain.AccountTraffic;
using MWM.Extensions.Authentication.JWT;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Phoenixnet.Extensions.Method;
using System.Linq;
using MWM.API.Account.Service.Domain.SyncDefaultAdvert;
using SendGrid;
using SendGrid.Helpers.Mail;
using NUlid;
using Phoenixnet.Extensions.Caching;
using MWM.API.Account.Service.Application.AccountTraffic.Contract;

namespace MWM.API.Account.Service.Application.AccountTraffic
{
    public class AccountTrafficService : IAccountTrafficService
    {
        private readonly IGenerateJwtTokenService _jwt;
        private readonly IAccountRepository _repository;
        private readonly IGenerateId _generate;
        private readonly ISyncDefaultAdvert _syncDefault;
        private readonly ICachingService _caching;
        //private readonly ISyncToken _sync;

        public AccountTrafficService(IGenerateJwtTokenService jwt, IAccountRepository repository, IGenerateId generate, ISyncDefaultAdvert syncDefaultAdvert, ICachingService caching)
        {
            _jwt = jwt;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _generate = generate ?? throw new ArgumentNullException(nameof(generate));
            _syncDefault = syncDefaultAdvert;
            _caching = caching;
            //_sync = sync;
        }

        public async Task<BasicResponse<LoginTrafficResponse>> LoginAsync(LoginTrafficRequest request)
        {
            var entity = await _repository.GetByName(request.login_name);
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (entity.LoginName == null)
                return new BasicResponse<LoginTrafficResponse>() { desc = "登入失敗", code = 399 };

            if (entity.State != 1)
                return new BasicResponse<LoginTrafficResponse>() { desc = "無效帳號", code = 397 };

            if (entity.LoginFailCount >= 3)
            {
                if (time - entity.LoginTime < 900)
                    return new BasicResponse<LoginTrafficResponse>() { desc = "登入失敗超過3次,鎖定15分鐘", code = 398 };

                await _repository.UpdateLoginFailCount(entity.Id, time, request.login_ip);
            }

            if (entity.Password != request.password.ToMD5())
            {
                await _repository.UpdateLoginFailCount(entity.Id, time, request.login_ip, entity.LoginFailCount + 1);
                return new BasicResponse<LoginTrafficResponse>() { desc = "登入失敗", code = 399 };
            }

            var userId = entity.Id;
            var secretId = entity.SecretId;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sid, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.PrimarySid,secretId),
                new Claim(ClaimTypes.Name, request.login_name),
                new Claim(ClaimTypes.Surname, ""),
                new Claim(ClaimTypes.Email, entity.Email)
            };

            var token = _jwt.GenerateJwtToken(claims, null);

            return new BasicResponse<LoginTrafficResponse>()
            {
                desc = "success",
                code = 200,
                data = new LoginTrafficResponse()
                {
                    login_name = request.login_name,
                    token = token,
                    account_id = entity.Id
                }
            };
        }

        public async Task<BasicResponse<int>> CreateAsync(TrafficRequest request)
        {
            var data = await _repository.GetByName(request.login_name);

            if (data.LoginName != null)
                return new BasicResponse<int>() { desc = "帳號重複", code = 301, data = 0 };

            var entity = request.ToEntity();
            entity.Id = await GenerateId();

            var count = await _repository.Create(entity);

            await _syncDefault.SyncRecords(entity.Id);

            if (count.Equals(0))
                return StateCodeHandler.ForCount(count, 0);

            return StateCodeHandler.ForCount<int>(count, entity.Id);
        }

        public async Task<BasicResponse> UpdateAsync(TrafficUpdateRequest request)
        {
            var data = await _repository.Get(request.account_id);

            if (data.LoginName == null)
                return new BasicResponse() { code = 302, desc = "無此帳號" };

            var datetime = DateTimeOffset.UtcNow;
            var date = Convert.ToInt32(datetime.ToString("yyyyMMdd"));
            var time = datetime.ToUnixTimeSeconds();

            var entity = request.ToEntity();
            entity.UpdateDate = date;
            entity.UpdateTime = time;

            var count = await _repository.Update(entity);

            var filter = string.Join(",", request.contact_account.Select(a => $" ({entity.Id}, '{a.account_name}', {(int)a.account_type}) "));
            var insertSql = $@"INSERT INTO contact_account (account_id, contact_account, account_type ) VALUES {filter} ";

            await _repository.UpdateContactAccount(entity.Id, insertSql);

            return StateCodeHandler.ForCount(count);
        }

        public async Task<PagingResponse<IEnumerable<TrafficResponse>>> GetAllTrafficAsync(TrafficListRequest request)
        {
            var paging = new Paging(request.page, request.size);
            var entity = request.ToEntity(paging.Offset);
            var (rows, total) = await _repository.GetTrafficAll(entity);

            var result = rows.ToResponse();
            paging.RowsTotal = total;

            return StateCodeHandler.ForPagingCount<IEnumerable<TrafficResponse>>((int)total, result, paging);
        }

        public async Task<BasicResponse<TrafficInfoResponse>> GetInfoAsync(int accountId)
        {
            var entity = await _repository.Get(accountId);
            var contactEntity = await _repository.GetContactAccount(accountId);

            var result = new TrafficInfoResponse()
            {
                id = entity.Id,
                login_name = entity.LoginName,
                email = entity.Email,
                cellphone = entity.Cellphone,
                secret_id = entity.SecretId,
                secret_key = entity.SecretKey,
                contact_account = contactEntity.Select(a => new ContactAccount()
                {
                    account_type = (ContactAccountType)a.account_type,
                    account_name = a.contact_account
                })
            };

            return StateCodeHandler.ForNull(result);
        }

        public async Task<BasicResponse> UpdatePasswordAsync(UpdateTrafficPasswordRequest request)
        {
            var data = await _repository.GetByName(request.login_name);

            if (data.Password != request.password_old.ToMD5())
                return new BasicResponse() { desc = "登入失敗", code = 399 };

            var entity = request.ToEntity();

            var updatecount = await _repository.Update(entity);

            return StateCodeHandler.ForCount(updatecount);
        }

        public async ValueTask<BasicResponse> ForgetPasswordAsync(string account, string mail, string domain)
        {
            var entity = await _repository.GetByName(account);

            if (entity.Id == 0 || entity.Email != mail)
                return new BasicResponse() { desc = "帳號或信箱錯誤", code = 301 };

            var key = entity.Id.ToString();
            var token = $"{entity.Id}_{Ulid.NewUlid(DateTimeOffset.UtcNow)}";
            var timer = new TimeSpan(0, 30, 0);
            var field = "token";

            if (await _caching.HExistAsync(key, field))
            {
                await _caching.HDelAsync(key, field);
            }

            await _caching.HSetAsync(key, field, token);

            var result = await _caching.ExpireAsync(key, timer);
            var response = await SendMail(mail, token, domain);

            return StateCodeHandler.ForBool(result);
        }

        public async ValueTask<BasicResponse> ForgetPasswordResetAsync(ResetPasswordRequest request)
        {
            var key = request.token.Split('_')[0];
            var field = "token";

            if (!await _caching.HExistAsync(key, field))
            {
                return new BasicResponse() { desc = "token錯誤", code = 301 };
            }

            if (await _caching.HGetAsync(key, field) != request.token)
            {
                return new BasicResponse() { desc = "token錯誤", code = 301 };
            }
            var accountId = int.Parse(key);
            var count = await _repository.ResetPassword(accountId, request.password.ToMD5());

            if (count > 0)
            {
                await _caching.HDelAsync(key, field);
            }

            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse> ResetPasswordAsync(int account_id)
        {
            var defaultPassword = "demo";
            var count = await _repository.ResetPassword(account_id, defaultPassword.ToMD5());
            return StateCodeHandler.ForCount(count);
        }


        /// <summary>
        /// 產生Account Id
        /// </summary>
        /// <returns></returns>
        public async Task<int> GenerateId()
        {
            int id, count;

            do
            {
                id = Math.Abs((int)_generate.GetId());
                count = await _repository.CheckId(id);
            } while (count == 1);

            return id;
        }

        /// <summary>
        /// 確認帳號名稱重複
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task<bool> CheckNameExistAsync(int id, string name)
        {
            bool result = false;

            var nameList = await _repository.GetNextLevelAccountName(id, 2, 0);

            if (nameList.Count() > 0)
                result = nameList.Select(a => a.CompareTo(name).Equals(0)).FirstOrDefault(z => z == true);

            return result;
        }

        private async ValueTask<Response> SendMail(string mail, string key, string domain)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("support@runclound.com.tw");
            var subject = "Run-Cloud 忘記密碼通知";
            var to = new EmailAddress(mail);
            var plainTextContent = "plainText and easy to do anywhere, even with C#";
            var url = $"{domain}/forget_reset?token={key}";
            var htmlContent = $"請於30分鐘內 點擊連結重置密碼  {url}";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response;
        }
    }
}
