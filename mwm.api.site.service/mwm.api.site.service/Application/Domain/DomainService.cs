using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Site.Service.Application.Common;
using MWM.API.Site.Service.Application.Common.CustomException;
using MWM.API.Site.Service.Domain.Domain;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Domain
{
    public class DomainService : IDomainService
    {

        private readonly ILogger<DomainService> _logger;
        private readonly IGenerateId _generate;
        private readonly IDomainRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IOptions<AppSettings> _setting;
        private readonly ISyncDomain _sync;

        private readonly List<string> defaultvalue;
        public DomainService(ILogger<DomainService> logger,
                             IGenerateId generate,
                             IDomainRepository repository,
                             IConfiguration configuration,
                             IOptions<AppSettings> setting,
                             ISyncDomain sync)
        {
            _logger = logger;
            _generate = generate;
            _repository = repository;
            _configuration = configuration;
            _setting = setting;
            _sync = sync;
            defaultvalue = _setting.Value.application.defaultvalue.domain.Split(",").ToList();
           
        }


        #region for video domain use
        public async Task<int> CheckDomainId(long id)
        {
            var result = await _repository.CheckDomainId(id);
            return result;
        }
        public async Task<BasicResponse> CreateDomain(VideoDomainRequest detail)
        {
            var sid = _setting.Value.application.merchantid.mwmtest;
            var datetime = DateTimeOffset.Now;
            var date = Convert.ToInt32(datetime.ToString("yyyyMMdd"));

            var data = new DomainEntity
            {
                Id = await GenId(MethodEnum.Domain),
                DomainName = detail.domain_name,
                Cdate = date,
                Ctime = datetime.ToUnixTimeSeconds(),
                Udate = date,
                Utime = datetime.ToUnixTimeSeconds(),
                State = 1
            };

            var result = await _repository.CreateDomain(data);

            //fetch api syncdomain
            var response = await _sync.SyncDomian(HttpMethod.Post, sid, data.DomainName, data.State);
            if (response.code == 200)
            {
                return StateCodeHandler.ForCount(result);
            }

            return new BasicResponse { code = 300, desc = "同步域名資料失敗" };
        }

        public async Task<BasicResponse<IEnumerable<DomainResponse>>> GetDomains()
        {
            var result = await _repository.GetDomains();
            
                var data = result.Select(d => new DomainResponse
                {
                    id = d.Id,
                    domain_name = d.DomainName,
                    cdate = d.Cdate,
                    ctime = d.Ctime,
                    udate = d.Udate,
                    utime = d.Utime,
                    state = d.State,
                });
                return new BasicResponse<IEnumerable<DomainResponse>> { code = 200, desc = "success", data = data };
        }

       
        public async Task<BasicResponse> UpdateDomain(List<VideoDomainRequest> request)
        {
            var turnOndomaincount = request.Count(d => d.state == 1);

            switch (turnOndomaincount)
            {

                case 0:
                    throw new MissParaException("請至少啟動一組域名");

                case 2:
                    throw new MissParaException("同時只能啟動一組域名");

                default:
                    var datas = new List<DomainEntity>();
                    var sid = _setting.Value.application.merchantid.mwmtest;
                    var datetime = DateTimeOffset.Now;
                    var date = Convert.ToInt32(datetime.ToString("yyyyMMdd"));

                    foreach (var vdetail in request)
                    {    //fetch api sync domain
                        await _sync.SyncDomian(HttpMethod.Post, sid, vdetail.domain_name, vdetail.state);
                        var data = new DomainEntity
                        {
                            Id = vdetail.id ==0 ? await GenId(MethodEnum.Domain):vdetail.id,
                            DomainName = vdetail.domain_name,
                            State = vdetail.state ==0 ? 99:vdetail.state ,
                            Cdate = date,
                            Ctime = datetime.ToUnixTimeSeconds(),
                            Udate = date,
                            Utime = datetime.ToUnixTimeSeconds()
                        };
                        datas.Add(data);
                    };
                    var result = await _repository.UpdateDomain(datas);
                    

                    return StateCodeHandler.ForCount(result);

            }
           

        }
        

        public async Task<BasicResponse> DeleteDomain(long id)
        {
            var sid = _setting.Value.application.merchantid.mwmtest;
            var result = await _repository.DeleteDomain(id);
            var data = await _repository.GetDomainById(id);
            //fetch api sync domain
            var response = await _sync.SyncDomian(HttpMethod.Delete, sid, data.DomainName, data.State);
            if (response.code == 200)
            {
                return StateCodeHandler.ForCount(result);
            }
            return new BasicResponse { code = 300, desc = "同步域名資料失敗" };
        }

        #endregion

        #region for website

        public Task<int> CheckWebsiteId(long id)
        {
            var result = _repository.CheckWebsiteId(id);
            return result;
        }

        public async Task<BasicResponse> CreateSiteDomain(WebSiteDomainRequest request,long accountid)
        {
            if (accountid == 0) { throw new TokenException(); };
            var datetime = DateTimeOffset.Now;
            var date = Convert.ToInt32(datetime.ToString("yyyyMMdd"));
            var defaultData = defaultvalue.Select(d => d.Split(":")[1]).ToList();

            var data = new WebSiteDomainEntity
            {
                Id = await GenId(MethodEnum.Website),
                AccountId = accountid,
                DispatchDomainId = await GetDispatchDomainID(request.account_id), //getaccountdomainid
                TemplateId = Convert.ToInt64(defaultData[0]),//request.template_id,
                PubDomain = defaultData[1],//request.public_domain,
                WebsiteDesc = defaultData[2],//request.website_desc,
                WebsiteName = defaultData[3],//request.website_name,
                Keyword = defaultData[4],//request.keyword,
                Cdate = date,
                Ctime = datetime.ToUnixTimeSeconds(),
                Udate = date,
                Utime = datetime.ToUnixTimeSeconds(),
                State = 1
            };

            var result = await _repository.CreateWebsiteDomain(data);
            if (result > 0)
                return StateCodeHandler.ForCount(result);


            return new BasicResponse { code = 301, desc = "failed" };

        }

        public async Task<BasicResponse> DeleteSiteDomain(long domainid)
        {   
                var data = new WebSiteDomainEntity {Id=domainid};
                var result = await _repository.DeleteWebsiteDomain(data);
                return StateCodeHandler.ForCount(result);
        }

        public async Task<PagingResponse<IEnumerable<WebSiteDomainReponse>>> GetSiteDomainById(long accountid, int pageoffset, int pagesize)
        {
            if (accountid == 0) { throw new MissParaException(); };
            var paging = new Paging(pageoffset, pagesize);
            var (rows, total) = await _repository.GetWebsiteDomainById(accountid,paging.Offset,paging.PageSize);

            var data = rows.Select(d => new WebSiteDomainReponse
            {
                id = d.Id,
                account_id = d.AccountId,
                dispatch_domain_id = d.DispatchDomainId,
                template_id = d.TemplateId,
                public_domain = d.PubDomain,
                website_desc = d.WebsiteDesc,
                website_name = d.WebsiteName,
                keyword = d.Keyword,
                cdate = d.Cdate,
                ctime = d.Ctime,
                udate = d.Udate,
                utime = d.Utime,
                state = d.State
            });
            
            return new PagingResponse<IEnumerable<WebSiteDomainReponse>> { code = 200, desc = "success", data = data, paging = paging };
        }

        public async Task<PagingResponse<IEnumerable<WebSiteDomainReponse>>> GetSiteDomains(int pageoffset, int pagesize)
        {
            var paging = new Paging(pageoffset,pagesize);
            var (rows, total) = await _repository.GetWebsiteDomains(paging.Offset, paging.PageSize);
           
                var data = rows.Select(d => new WebSiteDomainReponse
                {
                    id = d.Id,
                    account_id = d.AccountId,
                    dispatch_domain_id = d.DispatchDomainId,
                    template_id = d.TemplateId,
                    public_domain = d.PubDomain,
                    website_desc = d.WebsiteDesc,
                    website_name = d.WebsiteName,
                    keyword = d.Keyword,
                    cdate = d.Cdate,
                    ctime = d.Ctime,
                    udate = d.Udate,
                    utime = d.Utime,
                    state = d.State
                });
            return new PagingResponse<IEnumerable<WebSiteDomainReponse>> { code = 200, desc = "success", data = data,paging= paging};
           
        }

        public async Task<BasicResponse> UpdateSiteDomain(WebSiteDomainRequest request)
        {
            var datetime = DateTimeOffset.Now;
            var date = Convert.ToInt32(datetime.ToString("yyyyMMdd"));           
                var data = new WebSiteDomainEntity
                {
                    Id = request.id,
                    AccountId = request.account_id,
                    DispatchDomainId = request.dispatch_domain_id,
                    TemplateId = request.template_id,
                    PubDomain = request.public_domain??string.Empty,
                    WebsiteDesc = request.website_desc ?? string.Empty,
                    WebsiteName = request.website_name ?? string.Empty,
                    Keyword = request.keyword ?? string.Empty,
                    Udate = date,
                    Utime = datetime.ToUnixTimeSeconds(),
                    State = request.state
                };
                var result = await _repository.UpdateWebsiteDomain(data);
            _logger.LogInformation($"LOG FOR SITE-UPDATE VALUE:{data.TemplateId},Request-data{request.template_id}");
          return   StateCodeHandler.ForCount(result);
           
        }

        public async Task<BasicResponse<IEnumerable<WebSiteDomainReponse>>> GetWebdomainSettingByAccount(long accountid)
        {
            if(accountid == 0) { throw new MissParaException(); };
          
            var result = await _repository.GetWebdomainSettingByAccount(accountid);

            var data = result.Select(d => new WebSiteDomainReponse
            {
                id = d.Id,
                account_id = d.AccountId,
                dispatch_domain_id = d.DispatchDomainId,
                template_id = d.TemplateId,
                public_domain = d.PubDomain,
                website_desc = d.WebsiteDesc,
                website_name = d.WebsiteName,
                keyword = d.Keyword,
                cdate = d.Cdate,
                ctime = d.Ctime,
                udate = d.Udate,
                utime = d.Utime,
                state = d.State
            });

            return new BasicResponse<IEnumerable<WebSiteDomainReponse>> { code = 200, desc = "success", data = data};
        }


        #endregion

        #region for dispatch
        /// <summary>
        /// 創建website_domain時使用
        /// </summary>
        /// <param name="accountid"></param>
        /// <returns></returns>
        public async Task<long> GetDispatchDomainID(Int64 accountid)
        {
            var result = await _repository.GetDispatchDomainID(accountid);
            if (result <= 0)
            {
                result = 0;
            }
            return result;
        }

        public async Task<int> CheckDispatchId(long id)
        {
            var result = await _repository.CheckDispatchId(id);
            return result;
        }

        public async Task<BasicResponse<int>> GetDispatchDomainId(long accountid)
        {
            var result = await _repository.CheckDomainId(accountid);
            return new BasicResponse<int> { code = 200, desc = "success", data = result };
        }

        public async Task<BasicResponse<DispatchResponse>> GetDispatchDomainNameByAccount(long accountid)
        {
            var result = await _repository.GetDispatchDomainNameByAccount(accountid);
            if (result != null)
            {
                var data = new DispatchResponse
                {
                    id = result.Id,
                    account_id = result.AccountId,
                    domain_name = result.DomainName,
                    state = result.State
                };

                return new BasicResponse<DispatchResponse> { code = 200, desc = "success", data = data };

            }
            return new BasicResponse<DispatchResponse> { code = 300, desc = "failed", data = null };
        }

        public async Task<PagingResponse<IEnumerable<DispatchResponse>>> GetDispatchDomain(int page, int size)
        {
            if (page == 0 & size == 0)
            {
                return new PagingResponse<IEnumerable<DispatchResponse>> { code = 200, desc = $"請輸入分頁參數" };
            }

            var paging = new Paging(page, size);

            var (rows, total) = await _repository.GetDispatchDomain(paging.Offset, size);
            paging.RowsTotal =(int) total;
            
                var result = rows.Select(d => new DispatchResponse
                {
                    id = d.Id,
                    account_id = d.AccountId,
                    domain_name = d.DomainName,
                    cdate = d.Cdate,
                    ctime = d.Ctime,
                    udate = d.Udate,
                    utime = d.Utime,
                    state = d.State
                });
                var coUnt = result.Count();
                return StateCodeHandler.ForPagingCount<IEnumerable<DispatchResponse>>(coUnt, result, paging);
        }


        public async Task<BasicResponse> CreateDispatchData(DispatchRequest request)
        {
            var datetime = DateTimeOffset.Now;
            var date = Convert.ToInt32(datetime.ToString("yyyyMMdd"));

            var check = await _repository.CheckAccountId(request.account_id);
            if (check > 0)
            {
                var data = new DispatchEntity
                {
                    Id = await GenId(MethodEnum.Dipatch),
                    AccountId = request.account_id,
                    DomainName = GenDomain(),
                    Cdate = date,
                    Ctime = datetime.ToUnixTimeSeconds(),
                    Udate = date,
                    Utime = datetime.ToUnixTimeSeconds(),
                    State = 1
                };
                _logger.LogInformation($"accountid = {data.AccountId},system dispatch domain ={data.DomainName}");
                var result = await _repository.CreateDispatchData(data);

                return StateCodeHandler.ForCount(result);
            }

            return new BasicResponse { code = 300, desc = "帳號新增錯誤,無此帳號，請確認。" };
        }

        public async Task<int> CheckAccountId(long accountid)
        {
            var result = await _repository.CheckAccountId(accountid);
            return result;
        }
        #endregion


        protected async Task<long> GenId(MethodEnum method)
        {
            long id, result;
            switch (method)
            {
                case MethodEnum.Website:
                    id = _generate.GetId();
                    result = await CheckWebsiteId(id);
                    return result <= 0 ? id : 0;


                case MethodEnum.Domain:
                    id = _generate.GetId();
                    result = await CheckDomainId(id);
                    return result <= 0 ? id : 0;


                case MethodEnum.Dipatch:
                    id = _generate.GetId();
                    result = await CheckDispatchId(id);
                    return result <= 0 ? id : 0;

                default:
                    return 0;
            }
        }

        public string GenDomain()
        {
            var root = _setting.Value.application.rootdomain.phoenixnet;
            Guid G = Guid.NewGuid();
            var number = _generate.GetId().ToString().Substring(10, 3);
            var secondary = G.ToString().Substring(10, 3);
            var result = $"{number}.{secondary}.{root}";
            return result;
        }

       
    }
} 

