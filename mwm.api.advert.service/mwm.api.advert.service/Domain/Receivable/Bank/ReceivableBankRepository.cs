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
    public class ReceivableBankRepository : IReceivableBankRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IConfiguration _settings;
        private readonly ILogger<ReceivableBankRepository> _logger;

        public ReceivableBankRepository(IDbFactory dbFactory, IConfiguration settings, ILogger<ReceivableBankRepository> logger)
        {
            _dbFactory = dbFactory;
            _settings = settings;
            _logger = logger;
        }

        public async Task<int> Create(ReceivableBankEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=>
                {
                    string tsql = @"INSERT INTO receivable_bank (id,account_id, bank_name, bank_number, user_name, city, province, address,cdate,ctime,udate,utime,state) 
                                    VALUES (@Id,@AccountId, @BankName, @BankNumber, @UserName, @City, @Province, @Address,@Cdate,@Ctime,@Udate,@Utime,@State);";
                    var result = await a.ExecuteAsync(tsql, entity);

                    return result;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"INSERT BANK INFO ERROR:{ex.Message}");
                throw ex;
            }
        }

        public async Task<int> Update(ReceivableBankEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"UPDATE receivable_bank rb
                                    SET    
                                       rb.bank_name = IF(@BankName='',rb.bank_name,@BankName), 
                                       rb.bank_number = IF(@BankNumber='',rb.bank_number,@BankNumber), 
                                       rb.user_name = IF(@UserName='',rb.user_name,@UserName), 
                                       rb.city = IF(@City='',rb.city,@City), 
                                       rb.province =IF(@Province='',rb.province,@Province), 
                                       rb.address = IF(@Address='',rb.address,@Address), 
                                       rb.udate = @Udate,
                                       rb.utime =@Utime,
                                       rb.state = IF(@State=0,rb.state,@State)
                                    WHERE rb.id = @Id AND rb.account_id = @AccountId;";

                    var result = await a.ExecuteAsync(tsql,entity);
                    return result;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"UPDATE ERROR:{ex.Message},{entity.Id}");
                throw ex;
            }

          
        }

        public async Task<int> Delete(long id, long accountid, int state)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=>
                {
                    string tsql = @"UPDATE receivable_bank 
                            SET state = @state 
                            WHERE id = @id and account_id = @accountid;";
                    var result = await a.ExecuteAsync(tsql,new {id,accountid,state });
                    return result;
                });
              
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message},{id},{accountid},{state}");
                throw ex;
            }
        }

        public async ValueTask<IEnumerable<ReceivableBankEntity>> ReadAll(int state)
        {
            var tsql = @"SELECT      b.id as Id, b.account_id as AccountId,b.bank_name as BankName,b.bank_number as BankNumber,
                                     b.user_name as UserName,b.province as Province,b.address as Address,b.state as State
                         FROM        mwm_all.receivable_bank b
                         INNER JOIN  mwm_all.account a on a.id = b.account_id
                         WHERE       b.state <> @state
                         ORDER BY    b.ctime desc;";

            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    return await cn.QueryAsync<ReceivableBankEntity>(tsql, new { state });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"READ ALL ERROR:{ex.Message}");
                throw ex;
            }
        }

        public async ValueTask<ReceivableBankEntity> ReadById(long id, long accountid, int state)
        {
            var tsql = @"SELECT      b.id, b.account_id as AccountId,b.bank_name as BankName,b.bank_number as BankNumber,
                                     b.user_name as UserName,b.province as Province,b.address as Address,b.state as State
                           FROM       mwm_all.receivable_bank b
                           INNER JOIN mwm_all.account a on a.id = b.accountid
                           WHERE       a.id = @accountid AND b.state <> @state;";

            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    return await cn.QueryFirstOrDefaultAsync<ReceivableBankEntity>(tsql, new { id, accountid, state });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"READ BY id ERROR:{id},ex:{ex.Message}");
                return new ReceivableBankEntity();
            }
        }

        public async ValueTask<(IEnumerable<ReceivableBankEntity> entity, long total)> ReadByAccountId(long accountid, int state, int offset, int limit)
        {
            var totalSql = @"SELECT      count(*) as total
                             FROM        mwm_all.receivable_bank b
                             INNER JOIN  mwm_all.account a on a.id = b.account_id
                             WHERE       a.id = @accountId AND b.state <> @state;";

            var rowSql = @"SELECT      b.id, b.account_id as AccountId,b.bank_name as BankName,b.bank_number as BankNumber,
                                       b.user_name as UserName,b.province as Province,b.city as City,b.address as Address,b.state as State
                           FROM        mwm_all.receivable_bank b
                           INNER JOIN  mwm_all.account a on a.id = b.account_id
                           WHERE       a.id = @accountid AND b.state <> @state
                           ORDER BY    b.ctime desc;";

            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    var multiple = await cn.QueryMultipleAsync(totalSql+rowSql, new { accountid, state, offset, limit });
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();
                    var entity = await multiple.ReadAsync<ReceivableBankEntity>();

                    return (entity: entity, total: total); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"READBYID ERROR:{accountid},ex:{ex.Message}");
                throw ex;
            }
        }
    }
}
