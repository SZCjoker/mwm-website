using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.AdvertReport
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAdvertReportRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_str"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<(IEnumerable<AdvertReportListEntity> record, long count)> GetListReport(string query_str,int start_date,int end_date, int index, int size);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<(IEnumerable<AdvertReportDetailEntity> record, long count)> GetDetailReport(int account_id,int start_date,int end_date, int index, int size);
    }
}