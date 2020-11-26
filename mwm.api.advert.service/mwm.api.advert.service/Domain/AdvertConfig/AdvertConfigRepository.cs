using System.Threading.Tasks;
using Dapper;
using Phoenixnet.Extensions.Data;
using Phoenixnet.Extensions.Data.MySql;

namespace MWM.API.Advert.Service.Domain.AdvertConfig
{
    /// <summary>
    /// 
    /// </summary>
    public class AdvertConfigRepository:IAdvertConfigRepository
    {
        private readonly IDbFactory _dbFactory;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbFactory"></param>
        public AdvertConfigRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Sponsor_isEnable(){
            var tsql = $@"SELECT sponsor_isEnable FROM advert_config;";
            using (var cn = await _dbFactory.OpenConnectionAsync())
            {
               return await cn.QueryFirstOrDefaultAsync<bool>(tsql);
            }
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<int> Update_Record(bool enable)
        {
            var tsql = @"UPDATE `advert_config`
                            SET
                            `sponsor_isEnable`=@enable
                            WHERE `id` = 1;";
            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, new {enable});
            }
        }
    }
}