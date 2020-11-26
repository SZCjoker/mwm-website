using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.ReferReport
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReferReportRepository
    {
        /// <summary>
        /// 流量數據(顯示所有流量主)
        /// </summary>
        /// <param name="query_str"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<(IEnumerable<ReferReportAllEntity> record, long count)> GetAllReferReport(string query_str,int start_date,int end_date, int index, int size);
        
       /// <summary>
       /// 流量數據(顯示單一流量主)
       /// </summary>
       /// <param name="account_id"></param>
       /// <param name="start_date"></param>
       /// <param name="end_date"></param>
       /// <param name="index"></param>
       /// <param name="size"></param>
       /// <returns></returns>
        Task<(IEnumerable<ReferReportSiteMasterEntity>record, long count)> GetSiteMasterReferReport(int account_id,int start_date,int end_date, int index, int size);
       
       
       
       /// <summary>
       /// 流量數據(顯示單一流量主底下所有網域數據)
       /// </summary>
       /// <param name="account_id"></param>
       /// <param name="date"></param>
       /// <returns></returns>
       Task<IEnumerable<ReferReportSiteMasterDetailEntity>> GetSiteMasterDetailReferReport(int account_id,int date);
    }
}