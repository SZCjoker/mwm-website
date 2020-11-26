using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MWM.API.Site.Service.Domain.Site;
using Phoenixnet.Extensions.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Site
{
    public class SiteRepository : ISiteRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbFactory _dbFactory;
        private readonly ILogger<SiteRepository> _logger;
        

        public SiteRepository(IConfiguration configuration,
                                  IDbFactory dbFactory,
                                  ILogger<SiteRepository> logger)
        {
            _configuration = configuration;
            _dbFactory = dbFactory;
            _logger = logger;
        }

        public async Task<int> InitializeWebsite(SiteInfoEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string websitesql = @"INSERT INTO  website ( id , account_id ,  address , mail ,logo,contact,newest_address,eternal_address,publish_msg_top,publish_msg_bottom,cdate,ctime,udate,utime,state)
						              VALUES               ( @Id , @AccountId , @Address ,@Mail,@Logo,@Contact,@NewestAdd,@EternalAdd,@PubMsgTop,@PubMsgBottom,@Cdate,@Ctime,@Udate,@Utime,@State);";

                    return await a.ExecuteAsync(websitesql, entity);
                });
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw ex;
                //return 0;
            }
        }

        public async Task<int> DeleteWebsite(SiteInfoEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"UPDATE website ws
                                SET    state =2
                                WHERE  ws.account_id = @AccountId ";

                    return await a.ExecuteAsync(tsql, entity);
                });
            }

            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                //return 0;
                throw ex;
            }
        }
      
        public async Task<(IEnumerable<SiteInfoEntity> rows, long total)> GetWebsiteDetails(int pageoffset, int pagesize, string queryStr)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string totalsql = $@"SELECT COUNT(*) AS total
                                      FROM website ws
                                      WHERE 1=1 {queryStr};";

                    string tsql = $@"SELECT 
                                       ws.id as Id,ws.account_id as AccountId,ws.address as Address,ws.mail as Mail,
                                       ws.logo as Logo,ws.newest_address as NewestAdd,ws.eternal_address as EternalAdd,
                                       ws.publish_msg_top as PubMsgTop,ws.publish_msg_bottom as PubMsgBottom,ws.cdate as Cdate,
                                       ws.ctime asCtime,ws.udate as Udate,ws.utime as Utime,ws.state as State
                                FROM   website ws
                                WHERE 1=1
                                {queryStr}
                                ORDER BY  ws.utime desc
                                LIMIT @pageoffset ,@pagesize;";
                    _logger.LogInformation($"QUERY CONDITION{queryStr}");
                    var multiple = await a.QueryMultipleAsync(tsql + totalsql, new { pageoffset, pagesize });
                    var rows = await multiple.ReadAsync<SiteInfoEntity>();
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();

                    return (rows, total);
                });
            }

            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return (Enumerable.Empty<SiteInfoEntity>(),0);
            }


        }
       
        public async Task<SiteInfoEntity> GetWebsiteDetailsById(long accountid)
        {

            try
            {

                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"SELECT  ws.id as Id,ws.account_id as AccountId,ws.address as Address,ws.mail as Mail,
                                        ws.logo as Logo,ws.newest_address as NewestAdd,ws.eternal_address as EternalAdd,
                                        ws.publish_msg_top as PubMsgTop,ws.publish_msg_bottom as PubMsgBottom,ws.cdate as Cdate,
                                        ws.ctime asCtime,ws.udate as Udate,ws.utime as Utime,ws.state as State
                                FROM website ws
                                WHERE ws.account_id = @accountid AND ws.state <> 2";

                    var result = await a.QueryFirstOrDefaultAsync<SiteInfoEntity>(tsql, new { accountid });

                    _logger.LogInformation($"result{result}");
                    return result;
                });
            }

            catch(Exception ex)
            {
                _logger.LogInformation($"SITE GET BY ID ERROR{ex.Message}");
                //return null;
                throw ex;
            }
        }
       
        public async Task<int> UpdateWebsiteInfo(SiteInfoEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"UPDATE  website ws
                                SET     ws.address = IF(@Address='',ws.address,@Address),
                                        ws.mail =    IF(@Mail='',ws.mail,@Mail),
                                        ws.logo =    IF(@Logo='',ws.logo,@Logo),
                                        ws.newest_address = IF(@NewestAdd='',ws.newest_address,@NewestAdd),
                                        ws.eternal_address = IF(@EternalAdd='',ws.eternal_address,@EternalAdd),
                                        ws.publish_msg_top = IF(@PubMsgTop='',ws.publish_msg_top,@PubMsgTop),
                                        ws.publish_msg_bottom = IF(@PubMsgBottom='',ws.publish_msg_bottom,@PubMsgBottom),
                                        ws.udate = IF(@Udate=0,ws.udate,@Udate),
                                        ws.utime = IF(@Utime=0,ws.utime,@Utime),
                                        ws.state = @state 
                                WHERE ws.id = @Id";

                    return await a.ExecuteAsync(tsql, entity);
                });
            }

            catch(Exception ex)
            {
                _logger.LogError($"Insert ERROR : {ex.Message}");
                // return 0;
                throw ex;
            }

        }

        public async Task<int> CheckId(long id)
        {
            return await _dbFactory.OperateAsync(async a =>
            {
                string tsql = @"SELECT  COUNT(id) count
                                FROM website ws
                                WHERE ws.account_id = @id";

                var result = await a.QuerySingleAsync<int>(tsql, new { id });

                _logger.LogInformation($"result{result}");
                return result;
            } );
        }

        public async ValueTask<SiteInfoEntity> GetCompanySiteData(long managerid)
        {
            try
            {

                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"SELECT  ws.id as Id,ws.account_id as AccountId,ws.address as Address,ws.mail as Mail,
                                        ws.logo as Logo,ws.newest_address as NewestAdd,ws.eternal_address as EternalAdd,
                                        ws.publish_msg_top as PubMsgTop,ws.publish_msg_bottom as PubMsgBottom,ws.cdate as Cdate,
                                        ws.ctime asCtime,ws.udate as Udate,ws.utime as Utime,ws.state as State
                                FROM website ws
                                WHERE ws.account_id = @managerid AND ws.state <> 99";

                    var result = await a.QueryFirstOrDefaultAsync<SiteInfoEntity>(tsql, new { managerid });

                    _logger.LogInformation($"result{result}");
                    return result;
                });
            }

            catch (Exception ex)
            {
                _logger.LogInformation($"GET COMPANY DATA ERROR:{ex.Message}");
                //return null;
                throw ex;
            }
        }
    }
}