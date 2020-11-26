using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Phoenixnet.Extensions.Data.MySql;

namespace MWM.API.Site.Service.Domain.ReferReport
{
    /// <summary>
    /// 
    /// </summary>
    public class ReferReportRepository:IReferReportRepository
    {
        private readonly IDbFactory _dbFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbFactory"></param>
        public ReferReportRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_str"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>

        public async Task<(IEnumerable<ReferReportAllEntity> record, long count)> GetAllReferReport(string query_str, int start_date, int end_date, int index, int size)
        {

        
            var tsql = $@"SELECT 
                                 SQL_CALC_FOUND_ROWS
                                  ma.account_id as account_id,  
                                  ma.login_name as login_name,
                                  SUM(ma.flow) as flow, 
                                  SUM(ma.visit_data)as  visit_data, 
                                  c.urlcnt as domain_cnt
                                FROM
                                    refer_report AS ma
                                        INNER JOIN
                                    (SELECT 
                                        account_id, COUNT(refer_url) AS urlcnt
                                    FROM
                                        (SELECT DISTINCT
                                        account_id, refer_url
                                    FROM
                                        refer_report
                                    GROUP BY refer_url , account_id) AS dcnt
                                    GROUP BY account_id) 
                                    AS c ON ma.account_id = c.account_id
                                    where 1=1
                                    and  ma.date  between  @start_date and @end_date
                                    {query_str}
                                GROUP BY ma.account_id,ma.login_name
                                         LIMIT  @index, @size; 
                                SELECT Found_rows() AS cnt;";
            
            
            
            using (var cn = await _dbFactory.OpenConnectionAsync())
            {
                var multiple = await cn.QueryMultipleAsync(tsql, new {start_date,end_date,index, size});

                var resultRecord = await multiple.ReadAsync<ReferReportAllEntity>();

                var resultCnt = await multiple.ReadSingleAsync<long>();

                return (record: resultRecord, count: resultCnt);
            }
        }
        /// <summary>
        /// 流量數據(顯示單一流量主)
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<ReferReportSiteMasterEntity> record, long count)> GetSiteMasterReferReport(int account_id, int start_date, int end_date, int index, int size)
        {
   
            var tsql = $@"SELECT 
                                 SQL_CALC_FOUND_ROWS
                                    account_id as account_id,
                                    login_name,
                                    date as date,
                                    SUM(flow) AS flow,
                                    SUM(visit_data) AS visit_data
                                FROM
                                    refer_report
                                WHERE  account_id = @account_id
                                and  date  between  @start_date and @end_date
                                GROUP BY date,login_name
                                LIMIT  @index, @size; 
                                SELECT Found_rows() AS cnt;";
            
            
            
            using (var cn = await _dbFactory.OpenConnectionAsync())
            {
                var multiple = await cn.QueryMultipleAsync(tsql, new {account_id,start_date,end_date,index, size});

                var resultRecord = await multiple.ReadAsync<ReferReportSiteMasterEntity>();

                var resultCnt = await multiple.ReadSingleAsync<long>();

                return (record: resultRecord, count: resultCnt);
            }
        }
        /// <summary>
        /// 流量數據(顯示單一流量主底下所有網域數據)
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
      
        public async Task<IEnumerable<ReferReportSiteMasterDetailEntity>> GetSiteMasterDetailReferReport(int account_id, int date)
        {
            var tsql = $@"SELECT 
                        account_id as account_id,
                        refer_url,
                        login_name,
                        date as date,
                        flow AS flow,
                        visit_data AS visit_data
                    FROM
                        refer_report
                    WHERE 1=1
                     and   account_id = @account_id
                     and  date  =  @date
                    ;";
            using (var cn = await _dbFactory.OpenConnectionAsync())
            {
                return await cn.QueryAsync<ReferReportSiteMasterDetailEntity>(tsql,new{account_id,date});
            }
        }
    }
}