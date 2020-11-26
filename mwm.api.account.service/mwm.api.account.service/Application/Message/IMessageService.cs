using MWM.API.Account.Service.Application.Message.Contract;
using MWM.API.Account.Service.Application.Common;
using Phoenixnet.Extensions.Object;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Message
{
    public interface IMessageService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        ValueTask<PagingResponse<IEnumerable<MessageResponse>>> GetAll(int page, int rows);

        ValueTask<PagingResponse<IEnumerable<MessageResponse>>> GetAllForMerchant(long accountId, int page, int rows);

        ValueTask<PagingResponse<IEnumerable<MessageResponse>>> GetDetailByTopic(long topicId, int page, int rows);

        ValueTask<PagingResponse<IEnumerable<MessageResponse>>> QueryByCondition(QueryCondition condition);

        ValueTask<BasicResponse> ReadState(long topcid, int state);
        ValueTask<BasicResponse> CreateAsync(CreateUpdateMessageRequest request);
       
        ValueTask<BasicResponse> UpdateAsync(CreateUpdateMessageRequest request);
       
        ValueTask<BasicResponse> DeleteAsync(long id, long accountId);
    }
}
