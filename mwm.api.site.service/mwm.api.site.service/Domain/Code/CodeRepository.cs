using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Phoenixnet.Extensions.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Code
{
    public class CodeRepository : ICodeRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IConfiguration _settings;
        private readonly ILogger<CodeRepository> _logger;
       

        public CodeRepository(IDbFactory dbFactory, IConfiguration settings, ILogger<CodeRepository> logger)
        {
            _dbFactory = dbFactory;
            _settings = settings;
            _logger = logger;
        }

        public async Task<int> Create(CodeEntity entity)
        {
           

               
            try
            {
                return await _dbFactory.OperateAsync(async a=> {
                    string tsql = @"   INSERT INTO website_code (id,account_id, 51la, cnzz, ga, ally,cdate, ctime,udate,utime,state )
                                       VALUES     (@Id,@AccountId, @Code51la, @CodeCnzz, @CodeGA, @CodeAlly ,@Cdate, @Ctime,@Udate,@Utime,@State) ";

                    var result = await a.ExecuteAsync(tsql,entity);

                    return result;

                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

 
        }

        public async Task<int> Update(CodeEntity entity)
        {
            try
            {  return await _dbFactory.OperateAsync(async a=>
            
            {
                var tsql = @"UPDATE website_code  wc
                             SET    wc.51la = @Code51la,
                                    wc.cnzz = @CodeCnzz, 
                                    wc.ga =   @CodeGA,
                                    wc.ally=  @CodeAlly,
                                    wc.cdate = IF(@Cdate=0,wc.cdate,@Cdate), 
                                    wc.ctime = IF(@Ctime=0,wc.ctime,@Ctime),
                                    wc.state = IF(@State=0,wc.state,@State)
                             WHERE wc.account_id = @AccountId;";

                return await a.ExecuteAsync(tsql,entity);

            });
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"UPDATE ERROR:{ex.Message}");
                return 0;
            }

            
        }

        public async Task<int> Delete(long accountId)
        {
            var count = 0;
            var tsql = @"Update website_code 
                         SET state = 99
                         WHERE account_id = @accountId;";

            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = tsql;
                        cmd.Parameters.AddWithValue("@accountId", accountId);
                        count = await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DELETE ERROR:{ex.Message}");
                return 0;
            }

            return count;
        }

        public async ValueTask<(IEnumerable<CodeEntity> rows, long total)> ReadAll(int pageoffset, int pagesize)
        {
            try
            {                

           return await _dbFactory.OperateAsync(async a=> {


               string totalsql = @"SELECT COUNT(*) AS total
                                   FROM mwm_all.website_code b
                                   WHERE       b.state = 1;";
               
               string tsql = @" SELECT  b.id as Id,
                                        b.account_id as AccountId,
                                        b.51la as Code51la,
                                        b.cnzz as CodeCnzz,
                                        b.ga as CodeGA,
                                        b.ally  as CodeAlly,
                                        b.cdate as Cdate,
                                        b.ctime as Ctime,
                                        b.udate as Udate,
                                        b.utime as Utime,
                                        b.state as State   
                            FROM        mwm_all.website_code b
                            WHERE       b.state = 1
                            ORDER BY b.ctime desc
                            LIMIT @pageoffset,@pagesize;";
               var multiple = await a.QueryMultipleAsync(totalsql+ tsql,new { pageoffset,pagesize});
               var total = await multiple.ReadFirstOrDefaultAsync<long>();
               var rows = await multiple.ReadAsync<CodeEntity>();

               return (rows, total);
            }); 
            }
            catch (Exception ex)
            {
                _logger.LogError($"GET ALL ERROR:{ex.ToString()}");
                return (Enumerable.Empty<CodeEntity>(),0);
            }
        }

        public async ValueTask<CodeEntity> ReadByAccount(long accountid)
        {
            try
            {
                return await _dbFactory.OperateAsync(
                    
                    async a=> {

                        string tsql = @"SELECT      
                                        b.id as Id,
                                        b.account_id as AccountId,
                                        b.51la as Code51la,
                                        b.cnzz as CodeCnzz,
                                        b.ga as CodeGA,
                                        b.ally  as CodeAlly,
                                        b.cdate as Cdate,
                                        b.ctime as Ctime,
                                        b.udate as Udate,
                                        b.utime as Utime,
                                        b.state as State 
                            FROM        mwm_all.website_code b
                            WHERE b.account_id = @accountid and b.state <> 99";

                        var result = await a.QueryFirstOrDefaultAsync<CodeEntity>(tsql, new { accountid });

                        return result;
                    });
            }

            catch(Exception ex)
            {
                _logger.LogError($"READ By ACCOUNT ERROR:{ex.Message},{accountid}");

                return null;
            }
        }

        public async ValueTask<CodeEntity> GetCompanyCode(long managerid)
        {
            return await _dbFactory.OperateAsync(async a=>
            {
                string tsql = @"SELECT      
                                        b.id as Id,
                                        b.account_id as AccountId,
                                        b.51la as Code51la,
                                        b.cnzz as CodeCnzz,
                                        b.ga as CodeGA,
                                        b.ally  as CodeAlly,
                                        b.cdate as Cdate,
                                        b.ctime as Ctime,
                                        b.udate as Udate,
                                        b.utime as Utime,
                                        b.state as State 
                            FROM        mwm_all.website_code b
                            WHERE b.account_id = @managerid and b.state <> 99";

                var result = await a.QueryFirstOrDefaultAsync<CodeEntity>(tsql, new { managerid });

                return result;

            });
        }

       
    }
}