using System.Linq;
using System.Threading.Tasks;
using MWM.API.Site.Service.Application.AdvertReport.Contract;
using MWM.API.Site.Service.Domain.AdvertReport;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Site.Service.Application.AdvertReport
{
    /// <summary>
    /// 
    /// </summary>
    public class AdvertReportService:IAdvertReportService
    {
        private readonly IAdvertReportRepository _advertReportRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="advertReportRepository"></param>
        public AdvertReportService(IAdvertReportRepository advertReportRepository)
        {
            _advertReportRepository = advertReportRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login_name"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<IPagingResult<AdvertListResponse>> Get_List_Record(string login_name, int start_date, int end_date, int index, int size)
        {
            var requestPaging = new Paging(index, size);

            string querystr = string.Empty;
            if (!string.IsNullOrEmpty(login_name))
            {
                querystr = $"and ac.login_name like '%{login_name}%'";
            }

            var (record, count) =  await _advertReportRepository.GetListReport(querystr, start_date, end_date, requestPaging.Offset, size);
            return StateCodeHandler.PagingRecord(record.Select(s => new AdvertListResponse
            {
                account_id = s.account_id,
                login_name = s.login_name,
                pc_clicks = s.pc_clicks,
                pc_views = s.pc_views,
                mobile_clicks = s.mobile_clicks,
                mobile_views = s.mobile_views
           
            }), new Paging(requestPaging.PageIndex, requestPaging.PageSize, count));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<IPagingResult<AdvertDetailResponse>> Get_Detail_Record(int account_id, int start_date, int end_date, int index, int size)
        {
            var requestPaging = new Paging(index, size);
            
            var (record, count) = await _advertReportRepository.GetDetailReport(account_id,start_date,end_date,requestPaging.Offset,size);
            
            
            return StateCodeHandler.PagingRecord(record.Select(s => new AdvertDetailResponse
            {
                account_id = s.account_id,
                referer = s.referer,
                pc_clicks = s.pc_clicks,
                pc_views = s.pc_views,
                mobile_clicks = s.mobile_clicks,
                mobile_views = s.mobile_views
            }), new Paging(requestPaging.PageIndex, requestPaging.PageSize, count));
        }
    }
}