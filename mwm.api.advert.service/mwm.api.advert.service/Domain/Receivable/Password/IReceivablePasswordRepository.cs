using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.Receivable
{
    public interface IReceivablePasswordRepository
    {
        
        ValueTask<ReceivablePasswordEntity> ReadById(long id, long accountid, int state);

        ValueTask<ReceivablePasswordEntity> ReadByAccountId(long accountid, int state);

        ValueTask<(IEnumerable<ReceivablePasswordEntity>rows,long total)> ReadAll(int state,int pageoffset,int pagesize);

        Task<int> Create(ReceivablePasswordEntity entity);

        Task<int> Update(ReceivablePasswordEntity entity);

        Task<int> ResetPassword(long account_id, string nPwd);

        Task<int> Delete(long accountid, int state,string ip);

        Task<int> Check(long accountid);
    }
}
