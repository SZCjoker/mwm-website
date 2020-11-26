using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MWM.API.Site.Service.Domain.AccountHistory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using Phoenixnet.Extensions.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.AccountHistory
{
    public class AccountHistoryService : IAccountHistoryService
    {


        private readonly ILogger<AccountHistoryService> _logger;
        private readonly IAccountHistoryRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountHistoryService(ILogger<AccountHistoryService> logger,
                                     IAccountHistoryRepository repository,
                                     IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }




        protected async Task<string> Compare(string before, string after)
        {
            _logger.LogInformation(before);
            _logger.LogInformation(after);

            try
            {

                var oldJobj = JsonConvert.DeserializeObject<JObject>(before);
                var neWJobj = JsonConvert.DeserializeObject<JObject>(after);

                var chenged = string.Empty;
                //compare 
                var result = Object.Equals(oldJobj, neWJobj);
                _logger.LogInformation(result.ToString());
                if (result == false)
                    chenged = after;
                return chenged;
            }
            
            catch(Exception ex)
            {
                _logger.LogError($"Compare ERROR: {ex}");
                return ex.ToString();
            }
         }
           

        public async Task<PagingResponse<IEnumerable<HistoryResponse>>> GetAccountHistoryByCondition(QueryRequest request)
        {
            if (request.page_size == 0)
            {
                return new PagingResponse<IEnumerable<HistoryResponse>> { code = 200, desc = $"請輸入分頁參數" };
            }
            var paging = new Paging(request.page_offset, request.page_size);

            var queryStr = string.Empty;

            request.page_offset = paging.Offset;

            if (!string.IsNullOrEmpty(request.login_name)) queryStr += $" AND am.login_name LIKE '%{request.login_name}%'";
            if (!string.IsNullOrEmpty(request.category)) queryStr += $" AND ah.category LIKE '%{request.category}%'";

            if (!string.IsNullOrEmpty(request.action)) queryStr += $" AND ah.`action` LIKE '%{request.action}%'";

            if (!string.IsNullOrEmpty(request.begin_date)) queryStr += $" AND from_unixtime(ah.utime,'%Y%m%d') >= STR_TO_DATE({request.begin_date},'%Y%m%d')";

            if (!string.IsNullOrEmpty(request.end_date)) queryStr += $" AND from_unixtime(ah.utime,'%Y%m%d') <= STR_TO_DATE({request.end_date},'%Y%m%d')";

            _logger.LogInformation($"{queryStr}");

            var queryData = new QueryAllEntity 
            { 
                Action = request.action,
                Category = request.category,
                LoginName = request.login_name,
                BeginDate = request.begin_date,
                EndDate = request.end_date,
                PageOffset = request.page_offset,
                PageSize = request.page_size
            };

            var (rows, total) = await _repository.GetAccountHistoryByCondition(queryData, queryStr);
            paging.RowsTotal = total;
            var result = rows.Select(d => new HistoryResponse
                {
                    login_name = d.Loginname,
                    category = d.Category,
                    action = d.Action,
                    update_data = d.UpdateData,
                    before_data = d.BeforeData,
                    after_data = d.AfterData,
                    ip = d.Ip,
                    udate = d.Udate,
                    utime = d.Utime
                });

            var coUnt = result.Count();

            return StateCodeHandler.ForPagingCount<IEnumerable<HistoryResponse>>(coUnt, result, paging);
        }

        public async Task<BasicResponse<IEnumerable<HistoryResponse>>> GetAllHistory()
        {
            var result = await _repository.GetAllHistory();
            var data = result.Select(d => new HistoryResponse
           {    login_name = d.Loginname,
                account_id = d.AccountId,
                action = d.Action,
                category = d.Category,
                before_data = d.BeforeData,
                after_data = d.AfterData,
                update_data = d.UpdateData,
                ip = d.Ip,
                udate = d.Udate,
                utime = d.Utime,
                count = d.Count
            });
            return new BasicResponse<IEnumerable<HistoryResponse>>() {desc = "success",code = 200,data = data };
        }

        public async  Task<BasicResponse> SaveAccountHistory(HistoryRequest entity)
        {   
            if(entity.account_id == 0)
            {new BasicResponse { code = 401, desc = "token expired" };}
            if(string.IsNullOrEmpty(entity.control_name)||string.IsNullOrEmpty(entity.restful_method))
                return new BasicResponse { code = 300, desc = "記錄錯誤無動詞" };
            var act = entity.control_name.ToString() + '-' + entity.restful_method.ToString();
            var ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var datetime = DateTimeOffset.UtcNow;
            var date = Convert.ToInt32(datetime.ToString("yyyyMMdd"));
            var time = datetime.ToUnixTimeSeconds();
            
            var data = new AccountHistoryEntity
            {
                AccountId = entity.account_id,
                Action = act.ToUpper() ?? null,
                Category = entity.category,
                UpdateData = entity.update_data,
                BeforeData = entity.before_data,
                AfterData = await Compare(entity.before_data, entity.update_data)
            };
            data.Ip = ip;
            data.Udate = date;
            data.Utime = time;
            _logger.LogInformation($"data parameter to repository{data.AccountId},{data.Action},{data.BeforeData},{data.AfterData}");
            
            var result = await _repository.SaveAccountHistory(data);
            return StateCodeHandler.ForCount(result);

        }
    }
}
