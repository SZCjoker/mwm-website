using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Phoenixnet.Extensions.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.Bulletin
{
    public class BulletinRepository : IBulletinRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IConfiguration _settings;
        private readonly ILogger<BulletinRepository> _logger;

        public BulletinRepository(IDbFactory dbFactory, IConfiguration settings, ILogger<BulletinRepository> logger)
        {
            _dbFactory = dbFactory;
            _settings = settings;
            _logger = logger;
        }

        public async Task<int> Create(BulletinEntity entity)
        {
            
            try
            {
                return await _dbFactory.OperateAsync(async a=> 
                {
                    string tsql = $@"INSERT INTO bulletin (id,account_id,login_name, sequence,title, body,pic_path, cdate, ctime, udate, utime, state )
                          VALUES  (@Id, @AccountId,@LoginName,@Sequence, @Title, @Body,@PicPath, @CDate, @CTime, @UDate, @UTime, @State) ";

                    var result = await a.ExecuteAsync(tsql,entity);
                    return result;
                });
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"CREATE BULLET ERROR:{ex.Message}");
                throw ex;
                //return 0;
            }
        }

        public async Task<int> Update(BulletinEntity entity)
        {
           
          

            try
            {
                return await  _dbFactory.OperateAsync(async a=>
                {
                    string tsql = @"UPDATE bulletin 
                            SET sequence = @Sequence,
                                title = IF(@title='',title,@title), 
                                body = IF(@body='',body,@body), 
                                pic_path = IF(@picpath='',pic_path,@picpath),
                                udate = @udate,
                                utime = @utime , 
                                state = @state  
                            where id = @id";
                    var result = await a.ExecuteAsync(tsql,entity);
                    return result;
                } );
              
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"UPDATE BULLETIN ERROR:{ex.Message},{entity.Id},{entity.LoginName}");
                throw ex;
                //return 0;
            }

           
        }

        public async Task<int> Delete(long id, int state)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=> {

                    string tsql = @"Update bulletin 
                                  set state = @state 
                                  Where id = @id";
                    var result = await a.ExecuteAsync(tsql,new { id,state});
                    return result;
                });
              
            }
            catch (Exception ex)
            {
                _logger.LogError($"DELETE ERROR:{ex.Message}");
                throw ex;
                //return 0;
            }
        }

        public async ValueTask<IEnumerable<BulletinEntity>> ReadAll(string state)
        {
            var tsql = @"   SELECT      b.id,
                                        b.account_id as accountId,
                                        b.sequence as Sequence,
                                        b.title,
                                        b.body,
                                        b.pic_path,
                                        b.cdate,
                                        b.ctime,
                                        b.udate,
                                        b.utime,
                                        b.state    
                            FROM        mwm_all.bulletin b
                            WHERE       b.state >= @state ";

            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    return await cn.QueryAsync<BulletinEntity>(tsql, new { state });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ex:{ex.ToString()}");
                throw ex;
                //return Enumerable.Empty<BulletinEntity>();
            }
        }

        public async ValueTask<BulletinEntity> ReadById(long id, int state)
        {
            var tsql = @"SELECT      b.id,
                                     b.account_id as accountId,
                                     b.sequence as Sequence,
                                     b.title,
                                     b.body,
                                     b.pic_path,   
                                     b.cdate,
                                     b.ctime,
                                     b.udate,
                                     b.utime,
                                     b.state    
                          FROM       mwm_all.bulletin b
                          WHERE      b.id = @id and b.state <> @state ";

            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    return await cn.QueryFirstOrDefaultAsync<BulletinEntity>(tsql, new { id, state });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GET BY ID ERROR:{ex.Message}");
                throw ex;
               // return new BulletinEntity();
            }
        }

        public async ValueTask<(IEnumerable<BulletinEntity> entity, long total)> ReadAll(int state, int offset, int limit)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=> 
                {
                    string totalSql = @"SELECT  COUNT(*) AS total
                                        FROM    mwm_all.bulletin b
                                        WHERE   b.state = @state; ";

                    string rowSql = @"  SELECT       
                                        b.id,
                                        b.account_id as accountId,
                                        b.sequence as Sequence,
                                        b.title,
                                        b.body,
                                        b.pic_path,
                                        b.cdate,
                                        b.ctime,
                                        b.udate,
                                        b.utime,
                                        b.state    
                                        FROM        mwm_all.bulletin b
                                        WHERE       b.state = @state 
                                        ORDER BY    b.sequence desc
                                        LIMIT @offset,@limit;";

                    var multiple = await a.QueryMultipleAsync(totalSql + rowSql, new { state, offset, limit });
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();
                    var entity = await multiple.ReadAsync<BulletinEntity>();

                    return (entity: entity, total: total);
                });
            }
            catch (Exception ex)
            {   
                _logger.LogError($"GETALL ERROR:{ex.Message}");
                throw ex;
                // return (entity: Enumerable.Empty<BulletinEntity>(), total: 0);
            }
        }

        public async ValueTask<(IEnumerable<BulletinEntity> entity, long total)> Query(string title, int pageoffset, int page)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=>
                {
                    string totalSql = $@"SELECT  COUNT(*) AS total
                                        FROM    bulletin b
                                        WHERE b.title like '%{title}%' AND b.state <> 99;";
                    

                    string tsql = $@"SELECT b.id as Id ,b.account_id as AccountId, 
		                                   b.login_name as LoginName, b.sequence as Sequence,
		                                   b.title as Title, b.body as Body, b.pic_path as PicPath,
		                                   b.cdate as Cdate, b.ctime as Ctime, b.state as State
                                    FROM bulletin b
                                    WHERE b.title like '%{title}%' AND b.state <> 99
                                    ORDER BY b.sequence desc ,from_unixtime(b.ctime,'%Y%m%d')desc
                                    LIMIT @pageoffset,@page;";

                    var multiple = await a.QueryMultipleAsync(totalSql+tsql, new {pageoffset,page});
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();
                    var entity = await multiple.ReadAsync<BulletinEntity>();

                    return (entity,total);
                
                });
                    }
            catch(Exception ex)
            {
                _logger.LogError($"QUERY ERROR{title}");
                throw ex;
                //return (Enumerable.Empty<BulletinEntity>(), 0);
            }
        }
    }
}