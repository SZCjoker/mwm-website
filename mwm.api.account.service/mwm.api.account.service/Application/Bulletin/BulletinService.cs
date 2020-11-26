using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Account.Service.Application.Bulletin.Contract;
using MWM.API.Account.Service.Application.Common;
using MWM.API.Account.Service.Domain.Bulletin;
using MWM.API.Account.Service.Domain.Bulletin.CustomException;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Bulletin
{
    public class BulletinService : IBulletinService
    {
        protected readonly IBulletinRepository _repository;
        protected readonly IHttpContextAccessor _context;
        protected readonly IGenerateId _generate;        
        protected readonly IOptions<AppSettings> _settings;
        protected readonly ILogger<BulletinService> _logger;

        public BulletinService(IGenerateId generate, IBulletinRepository repository, ILogger<BulletinService> logger, IHttpContextAccessor context, IOptions<AppSettings> settings)
        {
            _repository = repository;
            _generate = generate;
            _context = context;
            _settings = settings;
            _logger = logger;
        }

        public async ValueTask<BasicResponse> CreateAsync(CreateUpdateBulletinRequest request, long accountid, string longiname)
        {
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var data = new BulletinEntity()
            {
                Id = _generate.GetId(),
                AccountId = accountid ,
                LoginName = longiname,
                Sequence =request.sequence,
                Body = request.body,
                PicPath = request.pic_path?? string.Empty,
                Title = request.title,
                Cdate = date,
                Ctime = time,
                State = request.state,
                Udate = date,
                Utime = time
            };
            if (data.AccountId == 0 || data.LoginName == null) throw new TokenException();
            var count = await _repository.Create(data);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse> UpdateAsync(CreateUpdateBulletinRequest request)
        {
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var data = new BulletinEntity()
            {
                Id = request.id,
                Sequence = request.sequence,
                Body = request.body ?? string.Empty,
                PicPath = request.pic_path?? string.Empty,
                Title = request.title ?? string.Empty,
                State = request.state,
                Udate = date,
                Utime = time
            };

            var count = await _repository.Update(data);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse> DeleteAsync(long id)
        {
            var count = await _repository.Delete(id, 99);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<PagingResponse<IEnumerable<BulletinResponse>>> GetAll(int page, int rows)
        {
            var paging = new Paging(page,rows);

            var (entity, total) = await _repository.ReadAll(1, paging.Offset, paging.PageSize);
            paging.RowsTotal = total;
            var data= entity.Select(d => new BulletinResponse()
            {
                account_id = d.Id,
                sequence = d.Sequence,
                body = d.Body,
                pic_path = d.PicPath,
                cdate = d.Cdate,
                ctime = d.Ctime,
                id = d.Id,
                state = d.State,
                title = d.Title,
                udate = d.Udate,
                utime = d.Utime
            });
            var coUnt = data.Count();
            return StateCodeHandler.ForPagingCount(coUnt,data,paging);    
        }

        public async ValueTask<BasicResponse<BulletinResponse>> GetById(long id)
        {
            var data = await _repository.ReadById(id, 99);

            if (data == null)
                return new BasicResponse<BulletinResponse>() { code = 300, desc = "no_match_data", data = null };

            var model = new BulletinResponse()
            {
                account_id = data.AccountId,
                sequence = data.Sequence,
                cdate = data.Cdate,
                ctime = data.Ctime,
                pic_path = data.PicPath,
                title =data.Title,
                body = data.Body,
                id = data.Id,
                state = data.State,
                udate = data.Udate,
                utime = data.Utime
            };

            return StateCodeHandler.ForBool<BulletinResponse>((data.Id != 0), model);
        }

        public async ValueTask<PagingResponse<IEnumerable<BulletinResponse>>> Query(string title, int pageoffset, int page)
        {
            var paging = new Paging(pageoffset, page);
            var (entity, total) = await _repository.Query(title,paging.Offset,paging.PageSize);
            paging.RowsTotal = total;
            var data = entity.Select(d => new BulletinResponse()
            {
                account_id = d.AccountId,
                login_name = d.LoginName,
                sequence = d.Sequence,
                body = d.Body,
                pic_path = d.PicPath,
                cdate = d.Cdate,
                ctime = d.Ctime,
                id = d.Id,
                state = d.State,
                title = d.Title,
                udate = d.Udate,
                utime = d.Utime
            }) ;
            var coUnt = data.Count();
            return StateCodeHandler.ForPagingCount(coUnt, data, paging);
        }
    }
}