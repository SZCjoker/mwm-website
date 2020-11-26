using Phoenixnet.Extensions.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MWM.API.Site.Service.Application.Template;


namespace MWM.API.Site.Service.Application.Template
{
 public  interface ITemplateService
    {
       Task<BasicResponse> CreteTemplate(TemplateRequest request);
       Task<BasicResponse> DeleteTemplate(int id);
       Task<BasicResponse<IEnumerable<TemplateResponse>>> GetTemplates();
       Task<BasicResponse<TemplateResponse>> GetTemplateById(int id);
       Task<BasicResponse> UpdateTemplate(TemplateRequest request);
       Task<BasicResponse<IEnumerable<TemplateResponse>>> QureybyCondition(QueryRequest request);
       Task<int> GenId();
       Task<int> CheckId(int id);
    }
}
