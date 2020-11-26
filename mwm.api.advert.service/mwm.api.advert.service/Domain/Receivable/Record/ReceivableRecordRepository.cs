using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MWM.API.Advert.Service.Domain.Receivable.Record;
using Phoenixnet.Extensions.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.Receivable
{
    public class ReceivableRecordRepository : IReceivableRecordRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IConfiguration _settings;
        private readonly ILogger<ReceivableRecordRepository> _logger;

        public ReceivableRecordRepository(IDbFactory dbFactory, IConfiguration settings, ILogger<ReceivableRecordRepository> logger)
        {
            _dbFactory = dbFactory;
            _settings = settings;
            _logger = logger;
        }

        public async Task<int> Create(RecordEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                    {
                        string tsql = @"INSERT INTO receivable_record (id,account_id,pay_amount,bank_id,is_notify,cdate,ctime,pay_date,pay_time,is_paid )
                                        VALUES  (@Id, @AccountId, @BankId,@PayAmount,@IsNotify,@Cdate,@Ctime,@Paydate,@Paytime,@IsPaid);";
                        var result = await a.ExecuteAsync(tsql, entity);
                        return result;
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError($"CREATE ERROR:{ex.Message}");
                throw ex;
            }
        }

        public async Task<int> Paid(RecordEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"UPDATE receivable_record r
                                    SET    r.account_id = @AccountId,r.pay_amount = @PayAmount,r.pay_account_id = @PayAccountId,
                                           r.is_notify = @IsNotify,r.pay_date = @PayDate, r.pay_time = @PayTime ,r.is_paid = @IsPaid  
                                    WHERE  r.id = @Id AND r.account_id = @AccountId;";

                    var result = await a.ExecuteAsync(tsql, entity);
                    return result;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"PAID ERROR{ex.Message}");
                throw ex;
            }
        }

        public async Task<int> Cancel(CancelEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync
                (async a =>
                {
                    string tsql = @"UPDATE receivable_record 
                         SET is_paid = @IsPaid,is_cancel=@IsCancel,cancel_reason=@CancelReason 
                         WHERE id = @Id AND account_id = @AccountId;";

                    var result = await a.ExecuteAsync(tsql, entity);
                    return result;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"CANCEL ERROR:{ex.Message}");
                throw ex;
            }
        }

        public async ValueTask<(IEnumerable<RecordEntity> entity, long total)> ReadAll(int pageoffset, int pagesize)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {

                    string totalSql = @"SELECT count(*) as total
                                        FROM receivable_record rr
                                        JOIN receivable_bank rb on rr.bank_id=rb.id
                                        JOIN account a on rb.account_id =a.id;";

                    string rowSql = @"SELECT a.login_name as AccountName,rr.account_id as AccountId ,rb.bank_name asBankName,
                                             rb.bank_number as BankNumber,rb.user_name as UserName,rb.address as BankAddress,
                                             rr.cdate as Cdate ,rr.ctime as Ctime,rr.pay_date as PayDate,
                                             rr.pay_time as PayTime,rr.is_notify As IsNotify,rr.is_paid as IsPaid
                                      From receivable_record rr 
                                      JOIN receivable_bank rb on rr.bank_id=rb.id
                                      JOIN account a on rb.account_id =a.id
                                      ORDER BY rr.ctime desc
                                      LIMIT @pageoffset,@pagesize;";

                    {
                        var multiple = await a.QueryMultipleAsync(totalSql + rowSql, new { pageoffset, pagesize });
                        var total = await multiple.ReadFirstOrDefaultAsync<long>();
                        var entity = await multiple.ReadAsync<RecordEntity>();

                        return (entity, total);
                    }

                });

            }
            catch (Exception ex)
            {
                _logger.LogError($"GET MECHANT RECORD ERROR:{ex.Message}");
                throw ex;
            }
        }

        public async ValueTask<RecordEntity> ReadById(long id)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=> 
                {
                    string tsql = @"  SELECT    a.login_name, r.id, r.account_id as accountId, r.bank_id as bankId,
                                        r.pay_time as PayTime , r.pay_amount as PayAmount, r.pay_account_id as payAccountId,
                                        r.is_notify as isNotify, r.cdate, r.ctime, r.pay_date, r.pay_time, r.is_paid,r.is_cancel,r.cancel_resaon
                                      FROM        receivable_record  r
                                      INNER JOIN  account a on a.id = r.account_id
                                      INNER JOIN  receivable_bank rb on rb.account_id = r.account_id
                                      WHERE       r.id =@id;";
                    var result = await a.QueryFirstOrDefaultAsync<RecordEntity>(tsql, new { id, });
                    return result;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"id:{id},GET BY ID ERROR:{ex.Message}");
                throw ex;
            }
        }

        public async ValueTask<(IEnumerable<RecordEntity> rows, long total)> ReadByAccountId(long accountid, int offset, int limit)
        {
            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    string totalSql = @"SELECT count(*) as total
                                        FROM receivable_record rr
                                        JOIN receivable_bank rb on rr.bank_id=rb.id
                                        JOIN account a on rb.account_id =a.id
                                        WHERE rr.account_id = @accountid;";

                    string rowSql = @"SELECT a.login_name as AccountName,rr.account_id as AccountId ,rb.bank_name asBankName,
                                             rb.bank_number as BankNumber,rb.user_name as UserName ,rb.address as BankAddress,rr.pay_account_id 
                                             as PayAccountId,rr.cdate as Cdate ,rr.ctime as Ctime,rr.pay_date as PayDate,rr.pay_time as PayTime,
                                             rr.is_notify As IsNotify,rr.is_paid as IsPaid 
                                      From receivable_record rr 
                                      JOIN receivable_bank rb on rr.bank_id=rb.id
                                      JOIN account a on rb.account_id =a.id
                                      WHERE rr.account_id = @accountid
                                      ORDER BY rr.ctime desc";
                    var multiple = await cn.QueryMultipleAsync(totalSql + rowSql, new { accountid, offset, limit });
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();
                    var entity = await multiple.ReadAsync<RecordEntity>();

                    return (entity, total);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GET MECHANT RECORD ERROR:{accountid},ex:{ex.Message}");
                throw ex;
            }
        }

        public async ValueTask<(IEnumerable<RecordEntity> entity, long total)> Query(QueryEntity entity, string queryStr)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = $@"SELECT a.login_name as AccountName,rb.bank_number as BankNumber,rb.bank_name as BankName,
                                            rb.user_name as UserName,r.id as Id,r.cdate as Cdate,r.ctime as Ctime,r.pay_date as PayDate,
                                            r.pay_time as PayTime ,r.pay_amount as PayAmount,r.pay_account_id as PayAccountId,r.is_paid as IsPaid
                                     FROM receivable_record r
                                     JOIN account a on r.account_id = a.id
                                     JOIN receivable_bank rb on a.id = rb.account_id
                                     WHERE 1=1 
                                     {queryStr}               
                                     ORDER BY from_unixtime(r.pay_time,'%Y%m%d') desc
                                     LIMIT @PageOffset,@Pagesize;";

                    string totalsql = $@"SELECT COUNT(*) AS total
                                         FROM receivable_record r
                                         JOIN account a on r.account_id = a.id
                                         JOIN receivable_bank rb on a.id = rb.account_id
                                         WHERE 1=1 
                                         {queryStr};";

                    var multible = await a.QueryMultipleAsync(tsql + totalsql, entity);
                    var rows = await multible.ReadAsync<RecordEntity>();
                    var total = await multible.ReadFirstOrDefaultAsync<long>();

                    return (rows, total);
                });

            }

            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw ex;
            }
        }


        public async Task<int> CheckByDate(long accountid, int ispaid, int date)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"SELECT COUNT(*)
                                    FROM receivable_record r
                                    WHERE r.account_id = @accountid AND r.is_paid =@ispaid
                                          AND r.cdate = @date;";

                    var result = await a.QueryFirstOrDefaultAsync<int>(tsql, new { accountid, ispaid, date });

                    return result;
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"CHECK ERROR:{ex.Message}");
                throw ex;
            }
        }

        public async ValueTask<(IEnumerable<MerchantCashOutRecordEntity> entity, long total)> MerchantGetRecord(long accountid, int offset, int limit)
        {
            try
            {
                using (var cn = await _dbFactory.OpenConnectionAsync())
                {
                    string totalSql = @"SELECT count(*) as total
                                        FROM receivable_record rr
                                        JOIN receivable_bank rb on rr.bank_id=rb.id
                                        JOIN account a on rb.account_id =a.id
                                        WHERE rr.account_id = @accountid;";

                    string rowSql = @"SELECT a.login_name as AccountName,rr.account_id as AccountId ,rb.bank_name as BankName,
                                             rb.bank_number as BankNumber,rb.user_name as UserName ,rb.address as BankAddress, 
                                             rr.cdate as Cdate ,rr.ctime as Ctime,rr.pay_date as PayDate,rr.pay_time as PayTime,
                                             rr.is_notify As IsNotify,rr.is_paid as IsPaid 
                                      From receivable_record rr 
                                      JOIN receivable_bank rb on rr.bank_id=rb.id
                                      JOIN account a on rb.account_id =a.id
                                      WHERE rr.account_id = @accountid
                                      ORDER BY rr.ctime desc
                                      LIMIT @offset,@limit;";
                    var multiple = await cn.QueryMultipleAsync(totalSql + rowSql, new { accountid, offset, limit });
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();
                    var entity = await multiple.ReadAsync<MerchantCashOutRecordEntity>();

                    return (entity, total);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GET MECHANT RECORD ERROR:{accountid},ex:{ex.Message}");
                throw ex;
            }

        }

        public async Task<int> Notify(long accountid, long recordid,int notify)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a=> 
                {
                    string tsql = @"UPDATE receivable_record r
                                    SET r.is_notify = @notify
                                    WHERE r.id =@recordid AND r.account_id = @accountid;";

                    var result = await a.ExecuteAsync(tsql,new { accountid,recordid,notify});
                    return result;
                });
            }
            catch(Exception ex)
            {
                _logger.LogError($"UPDATE NOTIFY ERROR:{ex.Message},{accountid},{recordid},{notify}");
                throw ex;
            }
        }
    }
}