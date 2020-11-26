using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.Receivable
{
    public interface IReceivableBankRepository
    {
        ValueTask<ReceivableBankEntity> ReadById(long id, long accountid, int state);

        ValueTask<(IEnumerable<ReceivableBankEntity> entity, long total)> ReadByAccountId(long accountid, int state, int offset , int limit);

        ValueTask<IEnumerable<ReceivableBankEntity>> ReadAll(int state);

        Task<int> Create(ReceivableBankEntity entity);

        Task<int> Update(ReceivableBankEntity entity);

        Task<int> Delete(long id, long accountid, int state);
    }
}
