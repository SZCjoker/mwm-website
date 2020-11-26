using Dapper;
using MWM.API.Account.Service.Domain.Account;
using Phoenixnet.Extensions.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.AccountTraffic
{
    public class AccountRepository : IAccountRepository
    {

        private readonly IDbFactory _dbFactory;

        public AccountRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// 紀錄帳號登入失敗次數
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeStamp"></param>
        /// <param name="ip"></param>
        /// <param name="failCount"></param>
        /// <returns></returns>
        public async Task<int> UpdateLoginFailCount(int id, long timeStamp, string ip, int failCount = 0)
        {
            return await _dbFactory.OperateAsync(async a =>
            {
                string tsql = @"UPDATE account t
                                   SET t.login_fail_count = @failCount,
                                       t.login_time = @timeStamp,
                                       t.login_ip = @ip
                                 WHERE t.id = @id";

                return await a.ExecuteAsync(tsql, new { id, failCount, ip, timeStamp });
            });
        }

        /// <summary>
        /// 建立帳號
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Create(AccountEntity entity)
        {
            return await _dbFactory.OperateAsync(async a =>
            {
                using (var transaction = a.BeginTransaction())
                {
                    string insertAccountSql = @"INSERT INTO account
                                                select @Id as `id`, 
                                                       @LoginName as `login_name`,
                                                       @Name as `name`,
                                                       @Email as `email`,
                                                       @Cellphone as `cellphone`,
                                                       MD5(@Password) as `password`,
                                                       @SecretId as `secret_id`,
                                                       @SecretKey as `secret_key`,
                                                       @CreateDate as `cdate`,
                                                       @CreateTime as `ctime`,
                                                       @UpdateDate as `udate`,
                                                       @UpdateTime as `utime`,
                                                       @LoginIp as `login_ip`,
                                                       @LoginTime as `login_time`,
                                                       @State as `state`,
                                                       @LoginFailCount as `login_fail_count`";
                    var count = await a.ExecuteAsync(insertAccountSql, entity, transaction);

                    transaction.Commit();

                    return count;
                }
            });
        }

        /// <summary>
        /// 更新帳號資訊
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Update(AccountEntity entity)
        {
            return await _dbFactory.OperateAsync(async a =>
            {
                string tsql = @"update account
                                   set email = if(@Email = '', email, @Email),
                                       cellphone = if(@Cellphone = '', cellphone, @Cellphone),
                                       password = if(@Password = '', password, MD5(@Password)),
                                       name = if(@Name = '', name, @Name),
                                       state = if(@State = 0, state, @State)
                                 where id = @Id";

                return await a.ExecuteAsync(tsql, entity);
            });
        }

        /// <summary>
        /// 更新聯絡資訊
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateContactAccount(int account_id, string insertSql)
        {
            string deleteSql = $@"delete from contact_account 
                                  where account_id = @account_id ";

            string tsql = $"{deleteSql} ; {insertSql}";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.ExecuteAsync(tsql, new { account_id });
            });
        }

        /// <summary>
        /// 重置密碼
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="defaultPassword"></param>
        /// <returns></returns>
        public async ValueTask<int> ResetPassword(int accountId, string defaultPassword)
        {
            string tsql = @"update account
                               set password = @defaultPassword
                             where id = @accountId";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.ExecuteAsync(tsql, new { accountId, defaultPassword });
            });
        }

        /// <summary>
        /// 變更帳號狀態(啟用、停用)
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<int> ChangeAccountState(int accountId, int state)
        {
            return await _dbFactory.OperateAsync(async a =>
            {
                var tsql = @"UPDATE account as A
                                SET A.state = @state
                              WHERE A.id = @accountId ";

                return await a.ExecuteAsync(tsql, new { state, accountId });
            });
        }

        /// <summary>
        /// 取得帳號資訊by帳號id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<AccountEntity> Get(int accountId)
        {
            return await _dbFactory.OperateAsync(async a =>
            {
                string tsql = @"SELECT  t.id as `Id` ,t.login_name as LoginName ,t.email as Email ,t.name as Name ,
                                        t.cellphone as Cellphone,t.password as `Password`,t.secret_id as SecretId,t.secret_key as SecretKey,
                                        t.cdate as CreateDate,t.ctime as CreateTime,t.udate as UpdateDate,t.utime as UpdateTime,
                                        t.login_ip as LoginIp,t.login_time as LoginTime,t.state as State,t.login_fail_count as LoginFailCount
                                FROM    account AS t
                                WHERE   t.id = @accountId";

                return await a.QuerySingleOrDefaultAsync<AccountEntity>(tsql, new { accountId });
            });
        }

        /// <summary>
        /// 取得帳號資訊by登入帳號
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<AccountEntity> GetByName(string account)
        {
            return await _dbFactory.OperateAsync(async a =>
            {
                string tsql = @"SELECT  t.id as `Id` ,t.login_name as LoginName ,t.email as Email ,t.name as Name ,
                                        t.cellphone as Cellphone,t.password as `Password`,t.secret_id as SecretId,t.secret_key as SecretKey,
                                        t.cdate as CreateDate,t.ctime as CreateTime,t.udate as UpdateDate,t.utime as UpdateTime,
                                        t.login_ip as LoginIp,t.login_time as LoginTime,t.state as State,t.login_fail_count as LoginFailCount
                                FROM    account AS t
                                WHERE   t.login_name = @account";

                return await a.QuerySingleOrDefaultAsync<AccountEntity>(tsql, new { account });
            });
        }

        /// <summary>
        /// 取得帳號聯絡資訊
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ContactAccountEntity>> GetContactAccount(int accountId)
        {
            string tsql = @"SELECT account_id, contact_account, account_type
                              FROM contact_account
                             WHERE account_id = @accountId";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.QueryAsync<ContactAccountEntity>(tsql, new { accountId });
            });
        }


        /// <summary>
        /// 確認帳號id是否重複
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> CheckId(int id)
        {
            return await _dbFactory.OperateAsync(async a =>
            {
                string tsql = @"SELECT COUNT(id) count FROM account WHERE id = @id";

                var count = await a.QuerySingleOrDefaultAsync<int>(tsql, new { id });
                return count;
            });
        }

        /// <summary>
        /// 取得所有下階帳號資訊(包含自己)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<AccountEntity> rows, long total)> GetTrafficAll(ListEntity entity)
        {
            return await _dbFactory.OperateAsync(async a =>
            {
                string tsql = @"SELECT SQL_CALC_FOUND_ROWS
                                       t.id as `Id` ,t.login_name as LoginName ,t.email as Email ,t.name as Name, dd.domain_name DomainName,
                                       t.cellphone as Cellphone,t.password as `Password`,t.secret_id as SecretId,t.secret_key as SecretKey,
                                       t.cdate as CreateDate,t.ctime as CreateTime,t.udate as UpdateDate,t.utime as UpdateTime,
                                       t.login_ip as LoginIp,t.login_time as LoginTime,t.state as State,t.login_fail_count as LoginFailCount
                                  FROM `account` t 
                                    left join website_dispatch_domain dd ON t.id = dd.account_id
                                 WHERE t.state = if(@State = 0, t.state, @State)
                                   AND t.login_name like if(@LoginName = '',t.login_name ,@LoginName)
                              ORDER BY t.id
                                 LIMIT @PageOffset ,@PageSize;
                                SELECT FOUND_ROWS() as cnt;";

                var multiple = await a.QueryMultipleAsync(tsql, entity);

                var rows = await multiple.ReadAsync<AccountEntity>();

                var total = await multiple.ReadSingleAsync<long>();

                return (rows, total);
            });
        }

        public async Task<IEnumerable<string>> GetNextLevelAccountName(int parentId, int isMerchant, int state, string filter = "")
        {
            return await _dbFactory.OperateAsync(async a =>
            {
                string sql = @"SELECT t.name
                                  FROM account t
                                 WHERE EXISTS ( SELECT 'Get NextLevelExceptSelf' 
                                                  FROM account a
                                 				 WHERE t.left > a.left
                                 				   AND t.right < a.right
                                 				   AND t.`level` = a.`level` + 1
                                 				   AND a.id = @parentId
                                 			  )
                                   AND t.is_merchant = if(@isMerchant = 0, t.is_merchant, @isMerchant)
                                   AND t.state = if(@state = 0, t.state, @state)";

                string tsql = $"{sql} {filter}";

                return await a.QueryAsync<string>(tsql, new { parentId, isMerchant, state });
            });
        }

        public async Task<int> RefreshUpdateTime(int accountId, int updateTime)
        {
            string tsql = @"update account
                               set utime = @updateTime
                             where id = @accountId";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.ExecuteAsync(tsql, new { accountId, updateTime });
            });
        }
    }
}
