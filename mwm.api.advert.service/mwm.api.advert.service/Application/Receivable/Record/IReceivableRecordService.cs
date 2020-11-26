using MWM.API.Advert.Service.Application.Common;
using MWM.API.Advert.Service.Application.Receivable.Contract;
using MWM.API.Advert.Service.Application.Receivable.Record;
using Phoenixnet.Extensions.Object;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Application.Receivable
{
    public interface IReceivableRecordService
    {       
        //company
        ValueTask<PagingResponse<IEnumerable<CashOutRecordAccountResponse>>> ReadByAccountId(long accountid, int page, int rows);
        //company
        ValueTask<PagingResponse<IEnumerable<RecordResponse>>> Query(QueryRequest request);
        //company
        ValueTask<PagingResponse<IEnumerable<CashOutRecordAccountResponse>>> GetAll(int page,int rows);
        //company
        ValueTask<BasicResponse<RecordResponse>> GetRecordById(long id);//類似水單號        
        //company
        ValueTask<BasicResponse> PaidAsync(PaidRequest request, long accountid);
        //company
        ValueTask<BasicResponse> NotifyAsync(long accountid, long recordid,int notify);
        //company 
        ValueTask<BasicResponse> CancelAsync(CancelRequest request);
        //merchant
        ValueTask<BasicResponse> ApplyRecordAsync(CreateRecordRequest request,long accountid);
        ValueTask<PagingResponse<IEnumerable<RecordResponse>>> MerchantGetRecord(long accountid,int page,int rows);
    }
}
