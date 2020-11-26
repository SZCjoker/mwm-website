using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Site.Service.Application.Common;
using MWM.API.Site.Service.Domain.Link;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Link
{
    public class LinkService : ILinkService
    {
        protected readonly ILinkRepository _repository;
        protected readonly IHttpContextAccessor _context;
        protected readonly IGenerateId _generate;
        protected readonly IOptions<AppSettings> _settings;
        protected readonly ILogger<LinkService> _logger;
        private readonly List<string> linkList;

        public LinkService(IGenerateId generate, ILinkRepository repository, ILogger<LinkService> logger, IHttpContextAccessor context, IOptions<AppSettings> settings)
        {
            _repository = repository;
            _generate = generate;
            _context = context;
            _settings = settings;
            _logger = logger;
            linkList = _settings.Value.application.defaultvalue.link.Split("|").ToList();

        }

        public async ValueTask<BasicResponse> CreateAsync(CreateUpdateLinkRequest request)
        {
            int count = 0;
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var linkDatas = linkList.Select(d => d.Split(","));

            foreach (var linkData in linkDatas)
            {
                var data = new LinkEntity()
                {
                    Id = _generate.GetId(),
                    AccountId = request.account_id,
                    Udate = date,
                    Utime = time,
                    Sequence =Convert.ToInt32(linkData[0]),//request.sequence,
                    Name = linkData[1],//request.name,
                    Link = linkData[2],//request.link,
                    Desc = linkData[3],//request.desc,
                    Cdate = date,
                    Ctime = time,
                    State = 1
                };
            count =  await _repository.Create(data);
            }            

            
           
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse> DeleteAsync(long id, long accountId)
        {
            var count = await _repository.Delete(id, accountId, 99);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse<IEnumerable<LinkResponse>>> GetAll(long accountid)
        {
            var result = await _repository.ReadAll(accountid);
            var data = result.Select(d => new LinkResponse
            { id = d.Id,
              account_id = d.AccountId,
              link = d.Link,
              name = d.Name,
              desc = d.Desc,
              sequence =d.Sequence,
              cdate = d.Cdate,
              ctime = d.Ctime,
              udate = d.Udate,
              utime = d.Utime,
              state = d.State
            });

            return new BasicResponse<IEnumerable<LinkResponse>> { code = 200, desc = "success", data = data };

        }

        public async ValueTask<BasicResponse<LinkResponse>> GetById(long id)
        {
            var data = await _repository.ReadById(id);

            if (data == null)
                return new BasicResponse<LinkResponse>() { code = 300, desc = "no_match_data", data = null };

            return StateCodeHandler.ForBool<LinkResponse>((data.Id != 0), new LinkResponse()
            {
                account_id = data.AccountId,
                cdate = data.Cdate,
                ctime = data.Ctime,
                udate = data.Udate,
                utime = data.Utime,
                desc = data.Desc,
                link = data.Link,
                name = data.Name,
                sequence = data.Sequence,
                id = data.Id,
                state = data.State
            });
        }

        public async ValueTask<BasicResponse> UpdateAsync(CreateUpdateLinkRequest request)
        {
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var data = new LinkEntity()
            {
                Id = request.id,
                AccountId = request.account_id,
                Udate = date,
                Utime = time,
                Desc = request.desc,
                Link = request.link,
                Name = request.name,
                Sequence = request.sequence,
                State = request.state
            };

            var count = await _repository.Update(data);
            return StateCodeHandler.ForCount(count);
        }
    }
}
