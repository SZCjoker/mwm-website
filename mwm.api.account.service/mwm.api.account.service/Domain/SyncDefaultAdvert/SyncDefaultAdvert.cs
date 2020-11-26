using System;
using System.Threading.Tasks;
using Dapper;
using Phoenixnet.Extensions.Data.MySql;

namespace MWM.API.Account.Service.Domain.SyncDefaultAdvert
{
    /// <summary>
    /// </summary>
    public class SyncDefaultAdvert : ISyncDefaultAdvert
    {
        private readonly IDbFactory _dbFactory;

        /// <summary>
        /// </summary>
        /// <param name="dbFactory"></param>
        public SyncDefaultAdvert(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// 同步預設廣告至流量主該表
        /// </summary>
        /// <returns></returns>
        public async Task<int> SyncRecords(int account_id)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var tsql = $@"INSERT INTO advert_traffic_master (account_id, default_ads_type,hyper_link,path,`desc`,banner_sort,position,ctime,utime,state)
                                 SELECT {account_id}, default_ads_type,hyper_link,path,`desc`,banner_sort,position,{create_time},{create_time},state FROM advert_default;";

            using (var cn = await _dbFactory.OpenConnectionAsync())
            {
                return await cn.ExecuteAsync(tsql);
            }
        }
    }
}