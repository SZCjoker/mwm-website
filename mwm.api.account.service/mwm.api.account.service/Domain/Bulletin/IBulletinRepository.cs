using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.Bulletin
{
    public interface IBulletinRepository
    {
        ValueTask<BulletinEntity> ReadById(long id, int state);

        ValueTask<(IEnumerable<BulletinEntity> entity, long total)> ReadAll(int state, int offset, int limit);
       

        Task<int> Create(BulletinEntity entity);

        Task<int> Update(BulletinEntity entity);

        Task<int> Delete(long id, int state);

        ValueTask<(IEnumerable<BulletinEntity> entity, long total)> Query(string title,int pageoffset,int page);
    }
}
