using System.Threading.Tasks;
using MWM.API.Site.Service.Application.AdvertReport.Contract;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Site.Service.Application.AdvertReport
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAdvertReportService
    {
        /// <summary>
        /// 廣告數據報表
        /// </summary>
        /// <param name="login_name"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<IPagingResult<AdvertListResponse>> Get_List_Record(string login_name,int start_date,int end_date, int index, int size);
        
        /// <summary>
        /// 廣告數據詳細報表
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<IPagingResult<AdvertDetailResponse>> Get_Detail_Record(int account_id,int start_date,int end_date, int index, int size); 
    }
}