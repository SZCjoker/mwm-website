using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Phoenixnet.Extensions.Data.MySql;

namespace MWM.API.Site.Service.Domain.AdvertReport
{
    /// <summary>
    /// 
    /// </summary>
    public class AdvertReportRepository:IAdvertReportRepository
    {
        private readonly IDbFactory _dbFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbFactory"></param>
        public AdvertReportRepository(IDbFactory dbFactory)
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

        public async Task<(IEnumerable<AdvertReportListEntity> record, long count)> GetListReport(string query_str, int start_date, int end_date, int index, int size)
        {
            
            
            var tsql = $@"SELECT 
                                SQL_CALC_FOUND_ROWS
                                    wd.account_id as account_id,
                                    ac.login_name as login_name,
                                    SUM(sw.pc_clicks) as pc_clicks ,
                                    SUM(sw.pc_views) as pc_views,
                                    SUM(sw.mobile_views) as mobile_views ,
                                    SUM(sw.mobile_clicks) as  mobile_clicks
                                FROM
                                    advert_web_summary AS sw
                                        INNER JOIN
                                    website_domain AS wd 
                                    ON sw.referer = wd.public_domain
                                    INNER JOIN `account` as ac
                                 ON   ac.id = wd.account_id 
                                 WHERE sw.cdate BETWEEN @start_date AND  @end_date
                                {query_str}
                                GROUP BY  wd.account_id
                                LIMIT  @index, @size; 
                                SELECT Found_rows() AS cnt;";
            
            
            
            using (var cn = await _dbFactory.OpenConnectionAsync())
            {
                var multiple = await cn.QueryMultipleAsync(tsql, new {start_date,end_date,index, size});

                var resultRecord = await multiple.ReadAsync<AdvertReportListEntity>();

                var resultCnt = await multiple.ReadSingleAsync<long>();

                return (record: resultRecord, count: resultCnt);
            }
            
            
      
        }

        public async Task<(IEnumerable<AdvertReportDetailEntity> record, long count)> GetDetailReport(int account_id, int start_date, int end_date, int index, int size)
        {
            var tsql = $@"SELECT 
                                    SQL_CALC_FOUND_ROWS
                                    wd.account_id as account_id,
                                    sw.referer as referer,
                                    SUM(sw.pc_clicks) as pc_clicks ,
                                    SUM(sw.pc_views) as pc_views,
                                    SUM(sw.mobile_views) as mobile_views ,
                                    SUM(sw.mobile_clicks) as  mobile_clicks
                                FROM
                                    advert_web_summary AS sw
                                        INNER JOIN
                                    website_domain AS wd 
                                    ON sw.referer = wd.public_domain
                                   WHERE
                                        sw.cdate BETWEEN @start_date AND @end_date
                                    and  wd.account_id =@account_id
                                GROUP BY sw.referer
                                LIMIT  @index, @size; 
                                SELECT Found_rows() AS cnt;";
            
            
            
            using (var cn = await _dbFactory.OpenConnectionAsync())
            {
                var multiple = await cn.QueryMultipleAsync(tsql, new {start_date,end_date ,account_id,index, size});

                var resultRecord = await multiple.ReadAsync<AdvertReportDetailEntity>();

                var resultCnt = await multiple.ReadSingleAsync<long>();

                return (record: resultRecord, count: resultCnt);
            }
        }
    }
}