using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Phoenixnet.Extensions.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Link
{
    public class LinkRepository : ILinkRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IConfiguration _settings;
        private readonly ILogger<LinkRepository> _logger;

        public LinkRepository(IDbFactory dbFactory, IConfiguration settings, ILogger<LinkRepository> logger)
        {
            _dbFactory = dbFactory;
            _settings = settings;
            _logger = logger;
        }

        public async Task<int> Create(LinkEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    var tsql = @"INSERT INTO website_link (id,account_id,sequence,`name`,link,`desc`,cdate,ctime,state)
                                 VALUES                   (@Id,@AccountId,@Sequence,@Name,@Link,@Desc,@Cdate,@Ctime,@State);";

                    var result = await a.ExecuteAsync(tsql ,entity);
                    return result;
                });
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"INSERT LINK ERROR:{ex.Message}");
                return 0;
            }

            
        }

        public async Task<int> Update(LinkEntity entity)
        {   
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    var tsql = @"UPDATE website_link wl
                                 SET wl.account_id = IF(@accountid=0,wl.account_id,@accountid),
                                     wl.sequence = IF(@sequence=0,wl.sequence,@sequence),
                                     wl.`name` = IF(@name='',wl.`name`,@name),
                                     wl.link = IF(@link='',wl.link,@link),
                                     wl.`desc`= IF(@desc='',wl.`desc`,@desc), 
                                     wl.udate = IF(@udate=0,wl.udate,@udate), 
                                     wl.utime = IF(@utime=0,wl.utime,@utime), 
                                     wl.state = IF(@state=0,wl.state,@state)  
                                WHERE wl.id = @id";

                    var result = await a.ExecuteAsync(tsql,entity);
                    return result;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"UPDATE LINK ERROR: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> Delete(long id, long accountId, int state)
        {
            var count = 0;
            var tsql = @"Update website_link 
                         set state = @state 
                         Where id = @id and account_id = @account_id ";

            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = tsql;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@account_id", accountId);
                        cmd.Parameters.AddWithValue("@state", state);

                        count = await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DELETE LINK ERROR:{ex.Message}");
                return 0;
            }

            return count;
        } 

        public async ValueTask<IEnumerable<LinkEntity>> ReadAll(long accountid)
        {
            var tsql = @"   SELECT      b.id,
                                        b.account_id as accountId,
                                        b.sequence,
                                        b.`name`,
                                        b.link,
                                        b.`desc`,
                                        b.cdate,
                                        b.ctime,
                                        b.udate,
                                        b.utime,
                                        b.state      
                            FROM        mwm_all.website_link b
                            WHERE       b.account_id = @accountid AND b.state <>99;";

            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    return await cn.QueryAsync<LinkEntity>(tsql, new { accountid });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"LINK READ ALL ERROR:{ex.Message}");
                return Enumerable.Empty<LinkEntity>();
            }
        }

        public async ValueTask<LinkEntity> ReadById(long id)
        {
            var tsql = @"SELECT         b.id,
                                        b.account_id as accountId,
                                        b.sequence,
                                        b.`name`,
                                        b.link,
                                        b.`desc`,
                                        b.cdate,
                                        b.ctime,
                                        b.udate,
                                        b.utime,
                                        b.state    
                            FROM        mwm_all.website_link b
                            WHERE       b.id = @id and b.state<>99;";

            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    return await cn.QueryFirstOrDefaultAsync<LinkEntity>(tsql, new { id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"LINK GET BY ID ERROR:{ex.Message}");
                return new LinkEntity();
            }
        }
    }
}
