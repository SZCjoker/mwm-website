using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Phoenixnet.Extensions.Data;
using Phoenixnet.Extensions.Data.MySql;

namespace MWM.API.Advert.Service.Domain.SponsorAdvertTrafficMaster
{
    /// <summary>
    /// 流量主-全民贊助廣告
    /// </summary>
    public class SponsorAdvertTrafficMasterRepository:ISponsorAdvertTrafficMasterRepository
    {
        private readonly IDbFactory _dbFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbFactory"></param>
        public SponsorAdvertTrafficMasterRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<(IEnumerable<SponsorAdvertTrafficMasterEntity> record, long count)> GetRecords(string query_str, int index, int size)
        {
            var tsql = $@"SELECT 
                                SQL_CALC_FOUND_ROWS
                                `id`,
                                `account_id`,
                                `image_link`,
                                `hyper_link`,
                                `position_type`,
                                `device_type`,
                                `image_link`,
                                `desc`,
                                `ctime`,
                                `utime`
                                FROM `advert_traffic_master_sponsor_print`
                                 {query_str}
                                LIMIT  @index, @size; 
                                SELECT Found_rows() AS cnt;
                                ";
            using (var cn = await _dbFactory.OpenConnectionAsync())
            {
                var multiple = await cn.QueryMultipleAsync(tsql, new {index, size});

                var resultRecord = await multiple.ReadAsync<SponsorAdvertTrafficMasterEntity>();

                var resultCnt = await multiple.ReadSingleAsync<long>();

                return (record: resultRecord, count: resultCnt);
            }
        }

        public async Task<int> Del_Record(int id)
        {
            var tsql = @"DELETE FROM `advert_traffic_master_sponsor_print` WHERE  `id`=@id ;";
            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, new {id});
            }
        }

        public async Task<int> Update_Record(SponsorAdvertTrafficMasterEntity entity)
        {
            var tsql = @"UPDATE `advert_traffic_master_sponsor_print`
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

        public async Task<int> Add_Record(SponsorAdvertTrafficMasterEntity entity)
        {
            var tsql = @"INSERT INTO  `advert_traffic_master_sponsor_print`
                        (
                        `account_id`,
                        `image_link`,
                        `hyper_link`,
                        `device_type`,
                        `position_type`,
                        `desc`,
                        `ctime`,
                        `utime`)
                        VALUES
                        (
                        @account_id,
                        @image_link,
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