using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Phoenixnet.Extensions.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.Receivable
{
    public class ReceivablePasswordRepository : IReceivablePasswordRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IConfiguration _settings;
        private readonly ILogger<ReceivablePasswordRepository> _logger;

        public ReceivablePasswordRepository(IDbFactory dbFactory, IConfiguration settings, ILogger<ReceivablePasswordRepository> logger)
        {
            _dbFactory = dbFactory;
            _settings = settings;
            _logger = logger;
        } 
 
        public async Task<int> Create(ReceivablePasswordEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=>
                { 
                    string tsql = $@"INSERT INTO receivable_password (id,account_id, password,login_ip,cdate,ctime,udate,utime,fail_count,state) 
                                     VALUES (@Id,@AccountId, @Password,@LoginIp,@Cdate,@Ctime,@Udate,@Utime,@FailCount,@State)
                                    ON DUPLICATE KEY UPDATE
                                    password=@Password,
                                    login_ip=@LoginIp,
                                    cdate=IF(@Cdate=0,cdate,@Cdate),
                                    ctime=IF(@Ctime=0,ctime,@Ctime),
                                    udate=IF(@Udate=0,udate,@Udate),
                                    utime=IF(@Utime=0,utime,@Utime),
                                    fail_count=@FailCount,
                                    state=@State;";
                               
                    var result = await a.ExecuteAsync(tsql,entity);

                    return result;
                });
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"INSERT PASSWORD ERROR:{ex.Message }");
                throw ex;
            }
        }

        public async Task<int> Delete(long accountid, int state,string ip)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=> 
                {
                    string tsql = @"UPDATE receivable_password 
                                    SET    state = @state,login_ip=@ip 
                                    WHERE  account_id = @accountid;";

                    var result = await a.ExecuteAsync(tsql,new { accountid,state,ip});
                    return result;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"UPDATE PASSWORD ERROR:{ex.Message},{ip}");
                throw ex;
            }
        }

        public async ValueTask<(IEnumerable<ReceivablePasswordEntity>rows,long total)> ReadAll(int state,int pageoffset,int pagesize)
        {   
            try
            {
                return await _dbFactory.OperateAsync(async a=>{ 
         
                    string tsql = @"SELECT      c.id, c.account_id as accountId, c.password as password,c.login_ip as LoginIp,
                                                c.udate as Cdate,c.utime as Ctime,c.fail_count as FailCount,c.state as State
                                    FROM        mwm_all.receivable_password c
                                    INNER JOIN  mwm_all.account a on a.id = c.account_id
                                    WHERE       c.state <> @state
                                    ORDER BY c.utime desc
                                    LIMIT @pageoffset,@pagesize;";

                    string totalsql = @"SELECT COUNT(c.id) count
                                        FROM mwm_all.receivable_password c 
                                        WHERE c.state <> @state;";


                    var multiple = await a.QueryMultipleAsync(totalsql + tsql, new { state, pageoffset, pagesize });
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();
                    var rows = await multiple.ReadAsync<ReceivablePasswordEntity>();
                    return (rows, total);
                });

            }
            catch (Exception ex)
            {
                _logger.LogError($"READLL ERROR:{ex.Message}");
                throw ex;
            }
        }

        public async ValueTask<ReceivablePasswordEntity> ReadById(long id, long accountid, int state)
        {
            var tsql = @" SELECT      c.id, c.account_id as accountId, c.password,c.udate,c.state
                          FROM        mwm_all.receivable_password c
                          INNER JOIN  mwm_all.account a on a.id = c.account_id
                          WHERE       a.id = @accountId and c.id = @id and c.state <> @state ";
            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    return await cn.QueryFirstOrDefaultAsync<ReceivablePasswordEntity>(tsql, new { id, accountid, state });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"READ BY id ERROR:{id},ex:{ex.Message}");
                throw ex;
            }
        }

        public async ValueTask<ReceivablePasswordEntity> ReadByAccountId(long accountid, int state)
        {
           
            try
            {
                return await _dbFactory.OperateAsync(async a => {
                string tsql = @"SELECT      c.id as Id, c.account_id as accountId, c.`password` as `Password` ,c.cdate as Cdate ,
                                            c.ctime as Ctime ,c.udate as Udate,c.utime as Utime, c.login_ip as LoginIp,
                                            c.fail_count as FailCount, c.state as State
                                FROM        mwm_all.receivable_password c
                                INNER JOIN  mwm_all.account a on a.id = c.account_id
                                WHERE       a.id = @accountid;";

                   var result = await a.QueryFirstOrDefaultAsync<ReceivablePasswordEntity>(tsql, new { accountid, state });
                  return result;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"READ BY accountId ERROR:{accountid},ex:{ex.Message}");
                throw ex;
            }
        }

       

        public async Task<int> Update(ReceivablePasswordEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=> 
                {
                    string tsql = @"UPDATE receivable_password r
                                    SET r.password = IF(@Password='',r.password,@Password),
                                        r.fail_count=IF(@FailCount=0,r.fail_count,@FailCount),
                                        r.login_ip = IF(@LoginIp='',r.login_ip,@LoginIp),
                                        r.udate = @Udate,
                                        r.utime = @Utime,
                                        r.state = @State
                                   WHERE r.account_id = @AccountId;";
                    var result = await a.ExecuteAsync(tsql, entity);
                    return result;                
                });
            }

            catch(Exception ex)
            {
                _logger.LogError($"PASSWORD UPDATE ERROR:{ex.Message}");
                throw ex;
            }
        }

        public async  Task<int> Check(long accountid)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"SELECT COUNT(*) 
                                    FROM receivable_password rp
                                    WHERE rp.account_id = @accountid AND rp.state <> 99 AND rp.state <> 0;";

                    var result = await a.QueryFirstOrDefaultAsync<int>(tsql, new { accountid});

                    return result;
                });
            }
            catch(Exception ex)
            {
                _logger.LogError($"CHECK PASSWORD EXIST ERROR:{ex.Message}");
                throw ex;
            }
        }

        public async Task<int> ResetPassword(long account_id,string nPwd)
        {
            try {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"UPDATE receivable_password rp
                                    SET rp.password = @nPwd,
                                        rp.state=0
                                    WHERE rp.account_id = @account_id;";

                    var result = await a.ExecuteAsync(tsql, new { account_id,nPwd });

                    return result;
            });
            }
            catch (Exception ex)
            {
                _logger.LogError($"EDIT PASSWORD ERROR:{ex.Message},{account_id}");
                throw ex;
            }

        }
    }
}
