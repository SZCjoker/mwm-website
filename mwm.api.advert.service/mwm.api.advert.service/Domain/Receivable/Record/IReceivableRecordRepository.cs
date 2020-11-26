using MWM.API.Advert.Service.Domain.Receivable.Record;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.Receivable
{
    public interface IReceivableRecordRepository
    {  //company 水單號
        ValueTask<RecordEntity> ReadById(long id);
        //merchant with token 
        ValueTask<(IEnumerable<MerchantCashOutRecordEntity> entity, long total)> MerchantGetRecord(long accountid, int offset, int limit);
        //both
        Task<int> CheckByDate(long accountid, int ispaid, int date);
        //company
        ValueTask<(IEnumerable<RecordEntity> rows, long total)> ReadByAccountId(long accountId, int offset, int limit);
        //company -list
        ValueTask<(IEnumerable<RecordEntity> entity,long total)> ReadAll(int pageoffset,int pagesize);
        //company
        ValueTask<(IEnumerable<RecordEntity> entity, long total)> Query(QueryEntity entity,string queryStr);
        //mechant
        Task<int> Create(RecordEntity entity);
        //company
        Task<int> Paid(RecordEntity entity);
        Task<int> Notify(long accountid, long recordid ,int notify);
        //company appied from merchabt .
        Task<int> Cancel(CancelEntity entity);

    }
}
