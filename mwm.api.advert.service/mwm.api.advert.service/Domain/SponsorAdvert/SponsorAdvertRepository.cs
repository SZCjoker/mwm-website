using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Phoenixnet.Extensions.Data;
using Phoenixnet.Extensions.Data.MySql;

namespace MWM.API.Advert.Service.Domain.SponsorAdvert
{
    /// <summary>
    ///  後台人員-操作贊助廣告
    /// </summary>
    public class SponsorAdvertRepository : ISponsorAdvertRepository
    {
        private readonly IDbFactory _dbFactory;

        /// <summary>
        ///     建構式
        /// </summary>
        /// <param name="dbFactory"></param>
        public SponsorAdvertRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        ///     後台人員-取贊助廣告列表
        /// </summary>
        /// <param name="query_str"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<SponsorAdverEntity> record, long count)> GetRecords(string query_str, int index, int size)
        {
            var tsql = $@"SELECT 
                                SQL_CALC_FOUND_ROWS
                                `id`,
                                `image_link`,
                                `hyper_link`,
                                `position_type`,
                                `device_type`,
                                `image_link`,
                                `desc`,
                                `ctime`,
                                `utime`
                                FROM `advert_sponsor_print`
                                 {query_str}
                                LIMIT  @index, @size; 
                                SELECT Found_rows() AS cnt;
                                ";
            using (var cn = await _dbFactory.OpenConnectionAsync())
            {
                var multiple = await cn.QueryMultipleAsync(tsql, new {index, size});

                var resultRecord = await multiple.ReadAsync<SponsorAdverEntity>();

                var resultCnt = await multiple.ReadSingleAsync<long>();

                return (record: resultRecord, count: resultCnt);
            }
        }

        /// <summary>
        ///     後台人員-刪除贊助廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Del_Record(int id)
        {
            var tsql = @"DELETE FROM `advert_sponsor_print` WHERE  `id`=@id ;";
            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, new {id});
            }
        }

        /// <summary>
        ///     後台人員-更新贊助廣告
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Update_Record(SponsorAdverEntity entity)
        {
            var tsql = @"UPDATE `advert_sponsor_print`
                            SET
                            `image_link`=@image_link,
                            `hyper_link`=@hyper_link,
                            `device_type`=@device_type,
                            `position_type`=@position_type,
                            `desc`=@desc,
                            `utime`=@utime
                            WHERE `id` = @id;";
            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, entity);
            }
        }

        /// <summary>
        ///     後台人員-新增贊助廣告
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Add_Record(SponsorAdverEntity entity)
        {
            var tsql = @"INSERT INTO  `advert_sponsor_print`
                        (`image_link`,
                        `hyper_link`,
                        `device_type`,
                        `position_type`,
                        `desc`,
                        `ctime`,
                        `utime`)
                        VALUES
                        (@image_link,
                        @hyper_link,
                        @device_type,
                        @position_type,
                        @desc,
                        @ctime,
                        @utime);";
            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, entity);
            }
        }
    }
}