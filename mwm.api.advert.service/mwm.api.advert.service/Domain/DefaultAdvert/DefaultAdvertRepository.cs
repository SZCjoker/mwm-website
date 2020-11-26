using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Phoenixnet.Extensions.Data;
using Phoenixnet.Extensions.Data.MySql;

namespace MWM.API.Advert.Service.Domain.DefaultAdvert
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultAdvertRepository:IDefaultAdvertRepository
    {
        private readonly IDbFactory _dbFactory;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbFactory"></param>
        public DefaultAdvertRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Add_Record(DefaultAdvertEntity entity)
        {
            var  tsql = @"INSERT INTO advert_default(`default_ads_type`,`hyper_link`,`position`,`image_link`,`desc`,`banner_sort`,`ctime`,`utime`,`state`)
                         VALUES(@default_ads_type,@hyper_link,@position,@image_link,@desc,@banner_sort,@ctime,@utime,@state);";
  

            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, entity);
            }

        }

        /// <summary>
        /// 更新單筆資料
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Update_Record(DefaultAdvertEntity entity)
        {

            var  tsql =  @"UPDATE advert_default
                            SET                  
                            `hyper_link` = @hyper_link,
                            `desc` = @desc,
                            `image_link`=@image_link,
                            `banner_sort`=@banner_sort,
                            `utime` = @utime,
                            `state` = @state
                            WHERE `id` = @id
                            AND  `default_ads_type` = @default_ads_type;
                            ";
            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, entity);
            }
        }

        /// <summary>
        /// 刪除單筆資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public async Task<int> Del_Record(int id,int types)
        {
            var  tsql =  @"DELETE FROM advert_default WHERE `id` = @id And default_ads_type =@types;";
            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, new{id,types});
            }
        }

        /// <summary>
        /// 取得列表
        /// </summary>
        /// <param name="query_str"></param>
        /// <param name="ads_type"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<DefaultAdvertEntity> record, long count)> GetRecords( string query_str,int ads_type, int index, int size)
        {
           
            var tsql = $@"SELECT  
                                SQL_CALC_FOUND_ROWS
                                id, 
                                default_ads_type, 
                                hyper_link, 
                                `image_link`, 
                                `desc`, 
                                banner_sort,
                                `position`, 
                                ctime, 
                                utime, 
                                state 
                                FROM advert_default
                                where default_ads_type =@ads_type
                                {query_str}
                                LIMIT  @index, @size; 

                                SELECT Found_rows() AS cnt;
                                ";

            using (var cn = await _dbFactory.OpenConnectionAsync())
            {
                var multiple = await cn.QueryMultipleAsync(tsql, new {ads_type,  index, size});

                var resultRecord = await multiple.ReadAsync<DefaultAdvertEntity>();

                var resultCnt = await multiple.ReadSingleAsync<long>();

                return (record: resultRecord, count: resultCnt);
            }
        }
    }
}