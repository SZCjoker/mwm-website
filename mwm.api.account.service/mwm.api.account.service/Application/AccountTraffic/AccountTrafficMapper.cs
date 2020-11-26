using MWM.API.Account.Service.Application.AccountTraffic.Contract;
using MWM.API.Account.Service.Domain.Account;
using NUlid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using NUlid;

namespace MWM.API.Account.Service.Application.AccountTraffic
{
    public static class AccountTrafficMapper
    {
        public static TrafficResponse ToResponse(this AccountEntity data)
        {
            return new TrafficResponse()
            {
                cellphone = data.Cellphone,
                cdate = data.CreateDate,
                ctime = data.CreateTime,
                email = data.Email,
                id = data.Id,
                login_fail_count = data.LoginFailCount,
                login_ip = data.LoginIp,
                login_name = data.LoginName,
                login_time = data.LoginTime,
                password = data.Password,
                secret_id = data.SecretId,
                secret_key = data.SecretKey,
                state = data.State,
                udate = data.UpdateDate,
                utime = data.UpdateTime
            };
        }

        public static AccountEntity ToEntity(this TrafficRequest data)
        {
            var datetime = DateTimeOffset.UtcNow;
            var date = Convert.ToInt32(datetime.ToString("yyyyMMdd"));
            var time = datetime.ToUnixTimeSeconds();
            var secret = Ulid.NewUlid().ToString();

            return new AccountEntity()
            {
                LoginName = data.login_name,
                Name = data.name ?? string.Empty,
                Email = data.email ?? string.Empty,
                Cellphone = data.cellphone ?? string.Empty,
                Password = data.password,
                LoginIp = string.Empty,
                SecretId = secret.Substring(16, 10),
                SecretKey = secret.Substring(0, 16),
                CreateDate = date,
                CreateTime = time,
                UpdateDate = date,
                UpdateTime = time,
                LoginTime = time,
            };
        }

        public static IEnumerable<TrafficResponse> ToResponse(this IEnumerable<AccountEntity> datas)
        {
            foreach (var data in datas)
            {
                yield return new TrafficResponse()
                {
                    cellphone = data.Cellphone,
                    cdate = data.CreateDate,
                    ctime = data.CreateTime,
                    email = data.Email,
                    id = data.Id,
                    login_fail_count = data.LoginFailCount,
                    login_ip = data.LoginIp,
                    login_name = data.LoginName,
                    login_time = data.LoginTime,
                    password = data.Password,
                    secret_id = data.SecretId,
                    secret_key = data.SecretKey,
                    state = data.State,
                    udate = data.UpdateDate,
                    utime = data.UpdateTime,
                    name = data.Name,
                    domain_name = data.DomainName
                };
            }
        }

        public static AccountEntity ToEntity(this TrafficUpdateRequest data)
        {
            return new AccountEntity()
            {
                Id = data.account_id,
                Email = data.email,
                Cellphone = data.cellphone,
                Password = data.password,
                Name = data.name,
                State = data.state
            };
        }

        public static AccountEntity ToEntity(this UpdateTrafficPasswordRequest data)
        {
            return new AccountEntity()
            {
                Id = data.account_id,
                Email = string.Empty,
                Cellphone = string.Empty,
                Password = data.password_new,
                Name = string.Empty
            };
        }

        public static ListEntity ToEntity(this TrafficListRequest data, int pageOffset)
        {
            return new ListEntity()
            {
                AccountId = data.account_id,
                State = data.state,
                LoginName = $"%{data.login_name}%" ?? string.Empty,
                PageOffset = pageOffset,
                PageSize = data.size,
                Order = data.orderby
            };
        }
    }
}
