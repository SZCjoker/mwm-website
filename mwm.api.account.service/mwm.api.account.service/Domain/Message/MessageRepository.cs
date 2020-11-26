using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Phoenixnet.Extensions.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.Message
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IConfiguration _settings;
        private readonly ILogger<MessageRepository> _logger;

        public MessageRepository(IDbFactory dbFactory, IConfiguration settings, ILogger<MessageRepository> logger)
        {
            _dbFactory = dbFactory;
            _settings = settings;
            _logger = logger;
        }

        public async Task<int> Create(MessageEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=>
                {
                    string tsql = @"INSERT INTO mwm_all.message (id, account_id,login_name,topic_id,title, body, type, cdate, ctime, state )
                                 VALUES  (@id, @accountid,@LoginName,@topicid,@Title, @body, @type, @cdate, @ctime, @state) ";
                    return await a.ExecuteAsync(tsql,entity);
                });
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"{entity.Id},{entity.TopicId},{entity.Title},{entity.Body}");
                throw ex;
                // return 0;
            }

           
        }

        public async Task<int> Update(MessageEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=>
                {
                    var tsql = $@"UPDATE message m
                                  SET m.title = IF(@Title='',m.title,@Title), 
                                      m.body = IF(@Body='',m.body,@Body),
                                      m.udate = IF(@Udate=0,m.udate,@Udate),
                                      m.utime = IF(@Utime=0,m.utime,@Utime), 
                                      m.state = @State)  
                                  WHERE m.id = @id AND m.account_id = @AccountId";

                    return await a.ExecuteAsync(tsql, entity);

                }); 
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"UPDATE ERROR {ex.Message}");
                throw ex;
                //return 0;
            }

           
        }

        public async Task<int> Delete(long id, long accountId)
        {   
            try
            {
                return await _dbFactory.OperateAsync(async a=> 
                {
                    var tsql = $@"UPDATE message 
                                  SET state = 99 
                                  WHERE id = @id and account_id = @account_id;";

                    return await a.ExecuteAsync(tsql, new { id,accountId});

                });
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"DELETE ERROR:{ex.Message},{id}");
                throw ex;
                //return 0;
            }
        }
                
       

        public async ValueTask<(IEnumerable<MessageEntity> entity, long total)> ReadAll(int offset, int limit)
        {
            var totalSql = @"SELECT  COUNT(*) AS total
                             FROM    mwm_all.message b                            
                             WHERE   b.state <> 99;";

            var rowSql = @" SELECT    b.id,
                                      b.account_id as accountId,
                                      b.login_name as LoginName,
                                      b.topic_id as topicId,
                                      b.title as Title,
                                      b.body,
                                      b.type,
                                      b.cdate,
                                      b.ctime,
                                      b.state     
                                      
                            FROM      mwm_all.message b
                            WHERE     b.state <> 99
                            ORDER BY  b.state,from_unixtime(b.ctime,'%Y%m%d') desc
                            LIMIT     @offset,@limit;";

            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    var multiple = await cn.QueryMultipleAsync(totalSql + rowSql, new { offset, limit });
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();
                    var entity = await multiple.ReadAsync<MessageEntity>();

                    return (entity: entity, total: total);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"ReadAll ex:{ex.Message}");
                throw ex;
               // return (entity: Enumerable.Empty<MessageEntity>(), total: 0);
            }
        }

        public async ValueTask<(IEnumerable<MessageEntity> entity, long total)> ReadAllForMerchant(long accountId, int offset, int limit)
        {
            var totalSql = @"   SELECT  COUNT(*) AS total
                                FROM    mwm_all.message b
                                WHERE   b.state <> 99  and b.title <> '' and b.account_id = @accountId; ";

            var rowSql = @"SELECT       b.id,
                                        b.account_id as accountId,
                                        b.login_name as LoginName, 
                                        b.topic_id as topicId,
                                        b.title as Title,
                                        b.body,
                                        b.type,
                                        b.cdate,
                                        b.ctime,
                                        b.state     
                            FROM        mwm_all.message b
                            WHERE       b.state <> 99 and b.title <> '' and b.account_id = @accountId LIMIT @offset,@limit;";

            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    var multiple = await cn.QueryMultipleAsync(totalSql + rowSql, new { accountId, offset, limit });
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();
                    var entity = await multiple.ReadAsync<MessageEntity>();

                    return (entity: entity, total: total);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"ReadAllForMerchant ex:{ex.Message}");
                throw ex;
                //return (entity: Enumerable.Empty<MessageEntity>(), total: 0);
            }
        }

        public async ValueTask<(IEnumerable<MessageEntity> entity, long total)> ReadDetailByTopic(long topicId, int offset, int limit)
        {
            var totalSql = @"SELECT  COUNT(*) AS total
                             FROM    mwm_all.message b
                             WHERE   b.state <>99 and b.topic_id = @topicId; ";

            var rowSql = @"SELECT    b.id,
                                     b.account_id as accountId,
                                     b.login_name as LoginName,   
                                     b.topic_id as topicId,
                                     b.title as Title,
                                     b.body,
                                     b.type,
                                     b.cdate,
                                     b.ctime,
                                     b.state     
                            FROM     mwm_all.message b
                            WHERE    b.state <> 99 and b.topic_id = @topicId LIMIT @offset,@limit;";

            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    var multiple = await cn.QueryMultipleAsync(totalSql + rowSql, new { topicId, offset, limit });
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();
                    var entity = await multiple.ReadAsync<MessageEntity>();

                    return (entity: entity, total: total);
                }
            }
             catch (Exception ex)
            {

                _logger.LogError($"ex:{ex.ToString()}");
                throw ex;
                //return (entity: Enumerable.Empty<MessageEntity>(), total: 0);
            }
        }

        public async ValueTask<(IEnumerable<MessageEntity> entity, long total)> QueryByCondition(MessageEntity entity,string queryStr, int pageoffset, int pagesize)
        {
            string totalsql = $@"SELECT COUNT(*) AS total 
                                 FROM mwm_all.message m
                                 JOIN mwm_all.account a on m.account_id = a.id
                                 WHERE 1=1 AND m.state <> 99 AND m.title <> ''
                                 {queryStr};";

            string rowsql = $@"SELECT   m.id,
                                        m.login_name as LoginName, 
                                        m.account_id as accountId,
                                        m.topic_id as topicId,
                                        m.title as Title,
                                        m.body,
                                        m.type,
                                        m.cdate,
                                        m.ctime,
                                        m.udate,
                                        m.utime,
                                        m.state 
                           FROM mwm_all.message m 
                           JOIN mwm_all.account a on m.account_id = a.id
                           WHERE 1=1 AND m.state <> 99  AND m.title <> ''
                           {queryStr} 
                           ORDER BY m.state, from_unixtime(m.ctime,'%Y%m%d') desc
                           LIMIT @pageoffset,@pagesize;";


            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    var multiple = await cn.QueryMultipleAsync( totalsql + rowsql, new { pageoffset,pagesize });
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();
                    var rows = await multiple.ReadAsync<MessageEntity>();

                    return (rows: rows, total: total);
                }

            }
            catch(Exception ex )
            {   
                _logger.LogError($"querystr:{queryStr},ex:{ex.ToString()}");
                throw ex;
                //  return (entity: Enumerable.Empty<MessageEntity>(), total: 0);

            }
        }

        public async Task<int> ReadState(long topicid,int state)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=>
                {
                    string tsql = @"UPDATE message m
                                    SET m.state = @state
                                    WHERE m.topic_id = @topicid;";

                    return await a.ExecuteAsync(tsql, new {topicid,state});
                
                });

            }
            catch (Exception ex)
            {
                _logger.LogError($"READ STATE ERROR:{ex.Message}");
                throw ex;
                //return 0;
            }
        }
    }
}
