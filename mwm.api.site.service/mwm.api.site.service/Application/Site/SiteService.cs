using Microsoft.Extensions.Logging;
using MWM.API.Site.Service.Application.Site.Datatype;
using MWM.API.Site.Service.Domain;
using MWM.API.Site.Service.Domain.Site;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using MWM.API.Site.Service.Application.Common;
using MWM.API.Site.Service.Application.Common.CustomException;

namespace MWM.API.Site.Service.Application.Site
{
    public class SiteService : ISiteService
    {

        private readonly ILogger<SiteService> _logger;
        private readonly ISiteRepository _repository;
        private readonly IGenerateId _generate;
        private readonly IOptions<AppSettings> _setting;
        private readonly List<string> value;


        public SiteService(ILogger<SiteService> logger,
                               ISiteRepository repository,
                               IGenerateId generate,IOptions<AppSettings> setting)
        {
            _logger = logger;
            _repository = repository;
            _generate = generate ?? throw new ArgumentException(nameof(generate));
            _setting = setting;
            value = _setting.Value.application.defaultvalue.site.Split("|").ToList();
        }

        public async Task<BasicResponse> CreateWebsite(WebDetailRequest request ,long accountid)
        {
            if (accountid == 0) { throw new TokenException();};
            var deafultData = value.Select(d => d.Split(":")).ToList();
            var datetime = DateTimeOffset.UtcNow;
            var date = Convert.ToInt32(datetime.ToString("yyyyMMdd"));
            var data = new SiteInfoEntity {
                Id = await GenerateId(),
                AccountId = accountid,
                Address = deafultData[0][1],//request.address??string.Empty,
                Mail = deafultData[1][1],//request.mail,
                Logo = deafultData[2][1],//request.logo,
                Contact = deafultData[3][1],//request.contact,
                NewestAdd = deafultData[4][1],//request.newest_address,
                EternalAdd = deafultData[5][1],//request.eternal_address,
                PubMsgTop = deafultData[6][1],//request.publish_msg_top,
                PubMsgBottom = deafultData[7][1],//request.publish_msg_bottom,
                Cdate = date,
                Ctime = datetime.ToUnixTimeSeconds(),
                Udate = date,
                Utime = datetime.ToUnixTimeSeconds(),
                State = 1
            };
            var result = await _repository.InitializeWebsite(data);
                
            return StateCodeHandler.ForCount(result);
           
        }

        public async Task<BasicResponse> DeleteWebsite(int accountid)
        {
          var data = new SiteInfoEntity { AccountId = accountid };
          var taRget = await _repository.GetWebsiteDetailsById(data.AccountId);
            if (taRget!=null )
            {
                var result = await _repository.DeleteWebsite(data);
                return StateCodeHandler.ForCount(result);
            }

            return new BasicResponse { code = 400, desc = "該網站已停權或輸入錯誤識別號" };

        }

        public async Task<PagingResponse<IEnumerable<WebDetailResponse>>> GetWebsite(int pagepffset, int pagesize)
        { 
            string queryStr = string.Empty;//save for the condition need ..
           
            if(pagepffset == 0 & pagesize == 0)
            return new PagingResponse<IEnumerable<WebDetailResponse>> { code = 200, desc = "請輸入分頁設定" };

           var paging = new Paging(pagepffset, pagesize);

           var (rows, total) =await _repository.GetWebsiteDetails(paging.Offset,paging.PageSize, queryStr);
          
            if (rows.Count() != 0)
            {
                var result = rows.Select(d => new WebDetailResponse
                {
                    id = d.Id,
                    account_id = d.AccountId==0? 0: d.AccountId,
                    address = d.Address,
                    mail = d.Mail,
                    logo = d.Logo,
                    contact = d.Contact ?? null,
                    newest_address = d.NewestAdd.Split(',').ToList() ?? null,
                    eternal_addres = d.EternalAdd.Split(',').ToList() ?? null,
                    publish_msg_bottom = d.PubMsgBottom,
                    publish_msg_top = d.PubMsgTop,
                    cdate = d.Cdate,
                    ctime = d.Ctime,
                    udate = d.Udate,
                    utime = d.Utime,
                    state = d.State
                }); 
                var coUnt = result.Count();
                return StateCodeHandler.ForPagingCount<IEnumerable<WebDetailResponse>>(coUnt, result, paging);
            }

            return new PagingResponse<IEnumerable<WebDetailResponse>> { code = 200, desc = "沒有符合的項目" };

        }

        public async Task<BasicResponse<WebDetailResponse>> GetWebsiteById(int accountid)
        {
            if (accountid == 0) { throw new TokenException(); };
            try
            {
                var result = await _repository.GetWebsiteDetailsById(accountid);

                var data = new WebDetailResponse
                {
                    id = result.Id,
                    account_id = result.AccountId,
                    address = result.Address,
                    mail = result.Mail,
                    logo = result.Logo,
                    contact = result.Contact,
                    newest_address = result.NewestAdd.Split(',').ToList() ?? null,
                    eternal_addres = result.EternalAdd.Split(',').ToList() ?? null,
                    cdate = result.Cdate,
                    ctime = result.Ctime,
                    udate = result.Udate,
                    utime = result.Utime,
                    state = result.State
                };

                return new BasicResponse<WebDetailResponse> { code = 200, desc = "success", data = data };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SITE SERVICE GET BY ID ERROR: {ex.Message}");
                return new BasicResponse<WebDetailResponse> { code = 300, desc = "failed", data = null };
            }
        }


        public async Task<BasicResponse> UpdateWebsite(WebDetailRequest request)
        {
            var datetime = DateTimeOffset.UtcNow;
            var date = Convert.ToInt32(datetime.ToString("yyyyMMdd"));

            var data = new SiteInfoEntity
            {
                Id = request.id,
                Address = request.address??string.Empty,
                Mail = request.mail ?? string.Empty,
                Logo = request.logo ?? string.Empty,
                Contact = request.contact ?? string.Empty,
                NewestAdd = request.newest_address ?? string.Empty,
                EternalAdd = request.eternal_address ?? string.Empty,
                PubMsgTop = request.publish_msg_top ?? string.Empty,
                PubMsgBottom = request.publish_msg_bottom ?? string.Empty,
                Udate = date,
                Utime = datetime.ToUnixTimeSeconds()
            };
            _logger.LogInformation($"UPDATE:{data.AccountId},{data.Address},{data.Mail},{data.PubMsgTop},{data.PubMsgBottom}");

            var result = await _repository.UpdateWebsiteInfo(data);

            return StateCodeHandler.ForCount(result);
        }

        public async Task<long> GenerateId()
        {   
                long id;
                id =  _generate.GetId();
                var result = await _repository.CheckId(id);

            return    result== 0 ? id:0;
        }

        public async ValueTask<BasicResponse<WebDetailResponse>> GetCompanySiteData(long managerid)
        {
            try
            {
                var result = await _repository.GetWebsiteDetailsById(managerid);

                var data = new WebDetailResponse
                {
                    id = result.Id,
                    account_id = result.AccountId,
                    address = result.Address,
                    mail = result.Mail,
                    logo = result.Logo,
                    contact = result.Contact,
                    newest_address = result.NewestAdd.Split(',').ToList() ?? null,
                    eternal_addres = result.EternalAdd.Split(',').ToList() ?? null,
                    cdate = result.Cdate,
                    ctime = result.Ctime,
                    udate = result.Udate,
                    utime = result.Utime,
                    state = result.State
                };

                return new BasicResponse<WebDetailResponse> { code = 200, desc = "success", data = data };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"GetCompanySiteData ERROR: {ex.Message}");
                // return new BasicResponse<WebDetailResponse> { code = 300, desc = "failed", data = null };
                throw ex;
            }

        }
    }
}
