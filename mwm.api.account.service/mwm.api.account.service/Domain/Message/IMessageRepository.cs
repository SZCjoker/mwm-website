using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.Message
{
    public interface IMessageRepository
    {
        ValueTask<(IEnumerable<MessageEntity> entity, long total)> ReadAll(int pageoffset, int pagesize);

        ValueTask<(IEnumerable<MessageEntity> entity, long total)> ReadAllForMerchant(long accountId, int pageoffset, int pagesize);

        ValueTask<(IEnumerable<MessageEntity> entity, long total)> ReadDetailByTopic(long topicId, int offset, int limit);

        ValueTask<(IEnumerable<MessageEntity> entity, long total)> QueryByCondition(MessageEntity entity,string queryStr,int pageoffset,int pagesize);
                
        Task<int> Create(MessageEntity entity);

        Task<int> Update(MessageEntity entity);

        Task<int> Delete(long id, long accountId);

        Task<int> ReadState(long topicid,int state );
    }
}
