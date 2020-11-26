using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Link
{
    public interface ILinkRepository
    {
        ValueTask<LinkEntity> ReadById(long id);

        ValueTask<IEnumerable<LinkEntity>> ReadAll(long accountid);

        Task<int> Create(LinkEntity entity);

        Task<int> Update(LinkEntity entity);

        Task<int> Delete(long id, long accountId, int state);
    }
}
