using System.Collections.Generic;
using System.Threading.Tasks;


namespace MWM.API.Site.Service.Domain.Template
{
    public interface ITemplateRepository
    {
        Task<int> CreateTemplate(TemplateEntity entity);
        Task<int> DeleteTemplate(TemplateEntity entity);
        Task<int> UpdateTemplate(TemplateEntity entity);
        Task<IEnumerable<TemplateEntity>> GetTemplates();
        Task<TemplateEntity> GetTemplateById(int id);
        Task<IEnumerable<TemplateEntity>> QuerybyCondition(string queryStr);
        Task<int> CheckId(int id);
    }
}
