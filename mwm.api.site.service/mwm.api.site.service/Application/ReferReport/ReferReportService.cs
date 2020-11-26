
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MWM.API.Site.Service.Application.ReferReport.Contract;
using MWM.API.Site.Service.Domain.ReferReport;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Site.Service.Application.ReferReport
{
    /// <summary>
    /// 
    /// </summary>
    public class ReferReportService:IReferReportService
    {
        private readonly IReferReportRepository _referReportRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referReportRepository"></param>
        public ReferReportService(IReferReportRepository referReportRepository)
        {
            _referReportRepository = referReportRepository;
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
        public async Task<IPagingResult<ReferReportAllResponse>> Get_ReferReportAll_Record(string login_name, int start_date, int end_date, int index, int size)
        {
            var requestPaging = new Paging(index, size);

            string querystr = string.Empty;
            if (!string.IsNullOrEmpty(login_name))
            {
                querystr = $"and ma.login_name like '%{login_name}%'";
            }
            
            var (record, count) = await _referReportRepository.GetAllReferReport(querystr,start_date,end_date,requestPaging.Offset,size);
            return StateCodeHandler.PagingRecord(record.Select(s => new ReferReportAllResponse
            {
                account_id = s.account_id,
                login_name = s.login_name,
                flow = s.flow,
                visit_data = s.visit_data,
                domain_cnt = s.domain_cnt
           
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
        public async Task<IPagingResult<ReferReportSiteMasterResponse>> Get_ReferReportSiteMaster_Record(int account_id, int start_date, int end_date, int index, int size)
        {
            var requestPaging = new Paging(index, size);
            
            var (record, count) = await _referReportRepository.GetSiteMasterReferReport(account_id,start_date,end_date,requestPaging.Offset,size);
           
            
            
            
            
            
            return StateCodeHandler.PagingRecord(record.Select(s => new ReferReportSiteMasterResponse
            {
                account_id = s.account_id,
                login_name = s.login_name,
                visit_data = s.visit_data,
                date = s.date,
                flow = s.flow

           
            }), new Paging(requestPaging.PageIndex, requestPaging.PageSize, count));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
      
        public async  Task<BasicResponse<IEnumerable<ReferReportSiteMasterDetailResponse>>> Get_ReferReportSiteMasterDetail_Record(int account_id, int date)
        {
            var result = await _referReportRepository.GetSiteMasterDetailReferReport(account_id,date);

            if (result==null)
            {
                return StateCodeHandler.ForNull<IEnumerable<ReferReportSiteMasterDetailResponse>>(null);
            }


            return StateCodeHandler.ForNull(result.Select(s=>new ReferReportSiteMasterDetailResponse()
            {
                    account_id = s.account_id, 
                    date = s.date, 
                    login_name = s.login_name, 
                    refer_url = s.refer_url, 
                    visit_data = s.visit_data, 
                    flow = s.flow
                
            } ));


        }
    }
}