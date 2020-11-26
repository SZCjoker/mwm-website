using Microsoft.Extensions.Logging;
using MWM.API.Site.Service.Domain.Template;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Template
{
    public class TemplateService : ITemplateService
    {

        private readonly ILogger<TemplateService> _logger;
        private readonly IGenerateId _generate;
        private readonly ITemplateRepository _repository;

        public TemplateService(ILogger<TemplateService> logger,
                               IGenerateId generate,
                               ITemplateRepository repository)
        {
            _logger = logger;
            _generate = generate;
            _repository = repository;
        }




        public async Task<int> CheckId(int id)
        {
            var result = await _repository.CheckId(id);
            return result;
        }

        public async Task<BasicResponse> CreteTemplate(TemplateRequest request)
        {
            var datetime = DateTimeOffset.Now;
            var date = Convert.ToInt32(datetime.ToString("yyyyMMdd"));
            var data = new TemplateEntity 
            {
                 Id = await GenId(),
                 Img = request.img,
                 Name =request.name,
                 Desc = request.desc,
                 AdvertAmount = request.advert_amount,
                 Cdate = date,
                 Ctime = datetime.ToUnixTimeSeconds(),
                 Udate = date,
                 Utime = datetime.ToUnixTimeSeconds(),
                 State = 1
            };

            var result = await _repository.CreateTemplate(data);
            return StateCodeHandler.ForCount(result);
        }

        public async Task<BasicResponse> DeleteTemplate(int id)
        {
            var check = await _repository.GetTemplateById(id);
            if (check != null)
            {
                var data = new TemplateEntity
                { Id = id};

                var result = await _repository.DeleteTemplate(data);
                return StateCodeHandler.ForCount(result);
            }

            return new BasicResponse {code=301,desc="無此模板" };

        }

        public async Task<int> GenId()
        {
            int id;
            id = Math.Abs((int)_generate.GetId());
            var result = await CheckId(id);
            return result <= 0 ? id : 0; 
        }

        public async Task<BasicResponse<TemplateResponse>> GetTemplateById(int id)
        {
            var result = await _repository.GetTemplateById(id);
            if(result != null)
            {
                var data = new TemplateResponse
                {  
                    id = result.Id,
                    img = result.Img,
                    name = result.Name,
                    desc = result.Desc,
                    advert_amount = result.AdvertAmount,
                    cdate = result.Cdate,
                    ctime = result.Ctime,
                    udate = result.Udate,
                    utime = result.Utime,
                    state = result.State
                };

                return new BasicResponse<TemplateResponse> { code = 200, desc = "success", data = data };
            }

            return new BasicResponse<TemplateResponse> { code = 301, desc = "該模板不存在請確認編號", data = null };
        }

        public async Task<BasicResponse<IEnumerable<TemplateResponse>>> GetTemplates()
        {
            var result = await _repository.GetTemplates();
            if(result !=null)
            {
                var data = result.Select(d => new TemplateResponse 
                {
                    id = d.Id,
                    img = d.Img,
                    name = d.Name,
                    desc = d.Desc,
                    advert_amount = d.AdvertAmount,
                    cdate = d.Cdate,
                    ctime = d.Ctime,
                    udate = d.Udate,
                    utime = d.Utime,
                    state = d.State
                });

                return new BasicResponse<IEnumerable<TemplateResponse>> { code = 200, desc = "success", data = data };
            }
            return new BasicResponse<IEnumerable<TemplateResponse>> { code = 301, desc = "failed", data = null };
        }

        public async Task<BasicResponse<IEnumerable<TemplateResponse>>> QureybyCondition(QueryRequest request)
        { 
            string queryStr = string.Empty;

            if (!string.IsNullOrEmpty(request.id)) queryStr += $" AND wt.id like'%{request.id}%'";
            if (!string.IsNullOrEmpty(request.name)) queryStr += $" AND wt.`name` like'%{request.name}%'";
            
            var entity = await  _repository.QuerybyCondition(queryStr);
            
            var data = entity.Select(d=> new TemplateResponse
            { 
                id = d.Id,
                img = d.Img,
                name = d.Name,
                cdate = d.Cdate,
                ctime = d.Ctime,
                state =d.State
            });
            
            return new BasicResponse<IEnumerable<TemplateResponse>> { code = 200, data = data, desc = "success" };
        }

        public async Task<BasicResponse> UpdateTemplate(TemplateRequest request)
        {
            var datetime = DateTimeOffset.Now;
            var date = Convert.ToInt32(datetime.ToString("yyyyMMdd"));
            var data = new TemplateEntity
            {
                Id = request.id,
                Img = request.img,
                Name = request.name,
                Desc = request.desc,
                AdvertAmount = request.advert_amount,
                Udate = date,
                Utime = datetime.ToUnixTimeSeconds(),
                State = request.state
            };            
            var result = await _repository.UpdateTemplate(data);
            return StateCodeHandler.ForCount(result);
        }
  
    
    
    
    
    
    
    
    
    
    
    
    
    
    }
}
