using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MWM.API.Account.Service.Application.Common;
using MWM.API.Account.Service.Application.Message;
using MWM.API.Account.Service.Application.Message.Contract;
using MWM.API.Account.Service.Domain.Message;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Bulletin
{
    public class MessageService : IMessageService
    {
        protected readonly IMessageRepository _repository;
        protected readonly IHttpContextAccessor _context;
        protected readonly IGenerateId _generate;        
        protected readonly IOptions<AppSettings> _settings;
        protected readonly ILogger<MessageService> _logger;

        public MessageService(IGenerateId generate, IMessageRepository repository, ILogger<MessageService> logger, IHttpContextAccessor context, IOptions<AppSettings> settings)
        {
            _repository = repository;
            _generate = generate;
            _context = context;
            _settings = settings;
            _logger = logger;
        }

        public async ValueTask<BasicResponse> CreateAsync(CreateUpdateMessageRequest request)
        {
            
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var uid = _generate.GetId();
            if (request.topic_id == 0) {
                                        request.topic_id = _generate.GetId();
                                       }//create message, topic wouldn't get value ,but reply does.

            var data = new MessageEntity()
            {
                Id = uid,
                AccountId = request.account_id,
                LoginName = request.login_name,
                Title = request.title,
                Body = request.body,
                TopicId = request.topic_id , 
                Type = request.type,
                Cdate = date,
                Ctime = time,
                State = 0  // 0 new open,1 service read ,2 user read,3 new msg from service(msg responsed) ,99 none 
            };

            var count = await _repository.Create(data);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse> UpdateAsync(CreateUpdateMessageRequest request)
        {
            var date = Convert.ToInt32(DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var data = new MessageEntity()
            {
                Id = request.id,
                AccountId = request.account_id,
                Title = request.title,
                Body = request.body,
                TopicId = request.topic_id,
                Type = request.state,
                Udate = date,
                Utime = time,
                State = request.state
            };

            var count = await _repository.Update(data);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<BasicResponse> DeleteAsync(long id, long accountId)
        {
            var count = await _repository.Delete(id, accountId);
            return StateCodeHandler.ForCount(count);
        }

        public async ValueTask<PagingResponse<IEnumerable<MessageResponse>>> GetAll(int page, int rows)
        {
            var paging = new Paging(page,rows);
            
            var (entity, total) = await _repository.ReadAll(paging.Offset, paging.PageSize);

            var datas = entity.Select(d => new MessageResponse()
            {
                id = d.Id,
                account_id = d.AccountId,
                topic_id = d.TopicId,
                title = d.Title,
                body = d.Body,
                type = d.Type,
                cdate = d.Cdate,
                ctime = d.Ctime,
                state = d.State,
                total = d.Total
            });
            paging.RowsTotal = total;
            var coUnt = entity.Count();
            return StateCodeHandler.ForPagingCount(coUnt, datas, paging);
        }
            

        public async ValueTask<PagingResponse<IEnumerable<MessageResponse>>> GetAllForMerchant(long accountId, int page, int rows)
        {
            var paging = new Paging(page, rows);
            var (entity, total) = await _repository.ReadAllForMerchant(accountId, paging.Offset, rows);
            paging.RowsTotal = total;
            var datas = entity.Select( d=> new MessageResponse()
                        {
                            id = d.Id,
                            account_id = d.AccountId,
                            title = d.Title,
                            topic_id = d.TopicId,
                            body = d.Body,
                            type = d.Type,
                            cdate = d.Cdate,
                            ctime = d.Ctime,
                            state = d.State
                        });
            var coUnt = datas.Count();
            return StateCodeHandler.ForPagingCount(coUnt,datas,paging);
        }

        public async ValueTask<PagingResponse<IEnumerable<MessageResponse>>> GetDetailByTopic(long topicId, int page, int rows)
        {
            var paging = new Paging(page,rows);
            var (entity, total) = await _repository.ReadDetailByTopic(topicId,paging.Offset, rows);
            paging.RowsTotal = total;
            var datas = entity.Select(d=> new MessageResponse()
                        {
                            id = d.Id,
                            login_name = d.LoginName,
                            account_id = d.AccountId,
                            topic_id = d.TopicId,
                            title = d.Title,
                            body = d.Body,
                            type = d.Type,
                            cdate = d.Cdate,
                            ctime = d.Ctime,
                            state = d.State
                        });

            var coUnt = datas.Count();
            return StateCodeHandler.ForPagingCount(coUnt, datas, paging);
        }

        public async  ValueTask<PagingResponse<IEnumerable<MessageResponse>>> QueryByCondition(QueryCondition condition)
        {  
            string queryStr = string.Empty;
            if(condition.pagesize==0)
            {
                return new PagingResponse<IEnumerable<MessageResponse>> {code=300,desc="請輸入分頁參數" ,data=null};
            }
            var paging = new Paging(condition.pageoffset, condition.pagesize);
            condition.pageoffset = paging.Offset;
            if (!string.IsNullOrEmpty(condition.login_name)) queryStr += $" AND a.login_name like '%{condition.login_name}%'";
            if (!string.IsNullOrEmpty(condition.title) ) queryStr += $" AND m.title like '%{condition.title}%'";
            if (condition.state.HasValue && condition.state != 2) { queryStr += $" AND m.state = {condition.state}"; }
            else if(condition.state == 2) { queryStr += $" AND m.state in(2,3)"; }
           
            var requestdata = new MessageEntity
            {
                 LoginName = condition.login_name,
                 State =  condition.state??0,
                 Title = condition.title??null
            };
            
            var (rows,total) =await _repository.QueryByCondition(requestdata, queryStr,paging.Offset,paging.PageSize);
            paging.RowsTotal = total;
            var datas = rows.Select(d => new MessageResponse
                {   
                    login_name = d.LoginName,
                    id = d.Id,
                    account_id = d.AccountId,
                    topic_id = d.TopicId,
                    title = d.Title,
                    body = d.Body,
                    type = d.Type,
                    cdate = d.Cdate,
                    ctime = d.Ctime,
                    udate = d.Udate,
                    utime = d.Utime,
                    state = d.State
                });

            var coUnt = datas.Count();
            return StateCodeHandler.ForPagingCount(coUnt, datas, paging);

        }

        public async ValueTask<BasicResponse> ReadState(long topicid, int state)
        {
            var result = await _repository.ReadState(topicid,state);

            return StateCodeHandler.ForCount(result);
        }
    }
}