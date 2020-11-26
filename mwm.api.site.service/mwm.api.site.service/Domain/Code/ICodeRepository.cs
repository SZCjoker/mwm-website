using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Code
{
    public interface ICodeRepository
    {

        ValueTask<CodeEntity> ReadByAccount(long accountid);
        
       // ValueTask<CodeEntity> GetTemplateCdoeByAccount(long accountid);
        
        ValueTask<CodeEntity> GetCompanyCode(long managerid);
                
        ValueTask<(IEnumerable<CodeEntity>rows,long total)> ReadAll(int pageoffset,int pagesize);

        Task<int> Create(CodeEntity entity);

        Task<int> Update(CodeEntity entity);

        Task<int> Delete(long accountId);
    }
}
