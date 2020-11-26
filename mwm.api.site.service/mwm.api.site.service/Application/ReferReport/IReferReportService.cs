using System.Collections.Generic;
using System.Threading.Tasks;
using MWM.API.Site.Service.Application.ReferReport.Contract;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Site.Service.Application.ReferReport
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReferReportService
    {
        /// <summary>
        /// 流量數據(顯示所有流量主)
        /// </summary>
        /// <param name="login_name"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<IPagingResult<ReferReportAllResponse>> Get_ReferReportAll_Record(string login_name,int start_date,int end_date, int index, int size);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<IPagingResult<ReferReportSiteMasterResponse>> Get_ReferReportSiteMaster_Record(int account_id,int start_date,int end_date, int index, int size); 
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<BasicResponse<IEnumerable<ReferReportSiteMasterDetailResponse>>> Get_ReferReportSiteMasterDetail_Record(int account_id,int date);
        
        
    }
}