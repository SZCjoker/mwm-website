using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Phoenixnet.Extensions.Data;
using Phoenixnet.Extensions.Data.MySql;

namespace MWM.API.Advert.Service.Domain.AdvertTrafficMaster
{
    /// <summary>
    /// 流量主後台-我的廣告
    /// </summary>
    public class AdvertTrafficMasterRepository:IAdvertTrafficMasterRepository
    {
        private readonly IDbFactory _dbFactory;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbFactory"></param>
        public AdvertTrafficMasterRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Add_Record(AdvertTrafficMasterEntity entity)
        {
            var  tsql = @"INSERT INTO advert_traffic_master(`account_id`,`default_ads_type`,`hyper_link`,`image_link`,`position`,`desc`,`banner_sort`,`ctime`,`utime`,`state`)
                         VALUES(@account_id,@default_ads_type,@hyper_link,@image_link,@position,@desc,@banner_sort,@ctime,@utime,@state);";
  

            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, entity);
            }
        }

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<int> Update_Record(AdvertTrafficMasterEntity entity,int account_id)
        {
            try
            {
                var  tsql =  $@"UPDATE advert_traffic_master
                            SET                  
                            `hyper_link` = @hyper_link,
                            `image_link` = @image_link,
                            `desc`=@desc,
                            `banner_sort`=@banner_sort,
                            `utime` = @utime,
                            `state` = @state
                            WHERE `id` = @id
                            AND `account_id`= {account_id}
                            AND  `default_ads_type` = @default_ads_type;
                            ";
                using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
                {
                    return await cn.ExecuteAsync(tsql, entity);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
 
        }

        public async Task<int> Del_Record(int id, int types)
        {
            var  tsql =  @"DELETE FROM advert_traffic_master WHERE `id` = @id And default_ads_type =@types;";
            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, new{id,types});
            }
        }

        public async Task<(IEnumerable<AdvertTrafficMasterEntity> record, long count)> GetRecords(int account_id, int ads_type, int index, int size)
        {
            var tsql = $@"SELECT  
                                SQL_CALC_FOUND_ROWS
                                id, 
                                account_id,
                                default_ads_type, 
                                hyper_link, 
                                `image_link`, 
                                `desc`, 
                                 banner_sort,
                                `position`, 
                                ctime, 
                                utime, 
                                state 
                                FROM advert_traffic_master
                                where default_ads_type =@ads_type
                                and account_id = @account_id
                                LIMIT  @index, @size; 

                                SELECT Found_rows() AS cnt;
                                ";

            using (var cn = await _dbFactory.OpenConnectionAsync())
            {
                var multiple = await cn.QueryMultipleAsync(tsql, new {ads_type,account_id,  index, size});

                var resultRecord = await multiple.ReadAsync<AdvertTrafficMasterEntity>();

                var resultCnt = await multiple.ReadSingleAsync<long>();

                return (record: resultRecord, count: resultCnt);
            }
        }
    }
}