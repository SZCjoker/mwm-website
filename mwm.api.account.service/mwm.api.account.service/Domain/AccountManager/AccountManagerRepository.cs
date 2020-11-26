using Dapper;
using MWM.API.Account.Service.Domain.Account;
using Phoenixnet.Extensions.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.AccountManager
{
    public class AccountManagerRepository : IAccountManagerRepository
    {
        private readonly IDbFactory _dbFactory;

        public AccountManagerRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// 取得帳號資訊by帳號
        /// </summary>
        /// <param name="login_name"></param>
        /// <returns></returns>
        public async ValueTask<ManageAccountEntity> GetByName(string login_name)
        {
            string tsql = @"SELECT id, login_name, `password`, role_id, login_ip, login_time, ctime, utime, state, login_fail_count
                              FROM account_manager
                             WHERE login_name = @login_name";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.QuerySingleOrDefaultAsync<ManageAccountEntity>(tsql, new { login_name });
            });
        }

        /// <summary>
        /// 取得帳號資訊by帳號id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async ValueTask<ManageAccountEntity> Get(int id)
        {
            string tsql = @"SELECT id, login_name, `password`, role_id, login_ip, login_time, ctime, utime, state, login_fail_count
                              FROM account_manager 
                             WHERE id = @id";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.QuerySingleOrDefaultAsync<ManageAccountEntity>(tsql, new { id });
            });
        }

        /// <summary>
        /// 取得帳號資訊清單
        /// </summary>
        /// <returns></returns>
        public async ValueTask<(IEnumerable<ManageAccountEntity> rows, long total)> GetAll(ListEntity entity)
        {
            string tsql = @"SELECT id, login_name, `password`, role_id, login_ip, login_time, ctime, utime, state, login_fail_count
                              FROM account_manager
                             where login_name like if(@login_name = '',login_name ,@login_name)
                             ORDER BY ctime
                             LIMIT @PageOffset ,@PageSize;
                            SELECT FOUND_ROWS() as cnt;";

            return await _dbFactory.OperateAsync(async a =>
            {
                var multiple = await a.QueryMultipleAsync(tsql, entity);
                var rows = await multiple.ReadAsync<ManageAccountEntity>();
                var total = await multiple.ReadSingleAsync<long>();
                return (rows, total);
            });
        }


        /// <summary>
        /// 建立帳號
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async ValueTask<int> Create(AccountManagerEntity entity)
        {
            string tsql = @"INSERT INTO account_manager (`id`, login_name, `password`, role_id, login_ip, login_time, ctime, utime, `state`, login_fail_count) 
                                   VALUES (@id, @login_name, MD5(@password), @role_id, @login_ip, @login_time, @ctime, @utime, @state, @login_fail_count);";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.ExecuteAsync(tsql, entity);
            });
        }

        /// <summary>
        /// 更新帳號資訊
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async ValueTask<int> Update(AccountManagerEntity entity)
        {
            string tsql = @"update account_manager
                               set password = if(@password = '', password, MD5(@password)),
                                   role_id = if(@role_id = 0, role_id, @role_id),
                                   utime = @utime
                             where id = @Id";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.ExecuteAsync(tsql, entity);
            });
        }

        /// <summary>
        /// 變更帳號狀態(啟用、停用)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public async ValueTask<int> ChangeAccountState(int id, int state, int time)
        {
            var tsql = @"UPDATE account_manager
                            SET state = @state,
                                utime = @time
                          WHERE id = @id ";

            return await _dbFactory.OperateAsync(async a =>
            {
                return await a.ExecuteAsync(tsql, new { state, id, time });
            });
        }

        /// <summary>
        /// 確認帳號id是否重複
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async ValueTask<int> CheckId(int id)
        {
            return await _dbFactory.OperateAsync(async a =>
            {
                string tsql = @"SELECT COUNT(id) count FROM account WHERE id = @id";

                var count = await a.QuerySingleOrDefaultAsync<int>(tsql, new { id });
                return count;
            });
        }

        public async ValueTask<(IEnumerable<AccountTrafficEntity> rows, long total)> GetAccountTraffic(string loginName, int state, int page, int size)
        {
            var tsql = $@"SELECT SQL_CALC_FOUND_ROWS
                                 t.id,
								 t.login_name,
								 t.email,
                                 t.cellphone,
								 t.secret_id,
								 t.secret_key,
								 t.ctime,
								 t.utime,
                                 t.login_ip,
								 t.login_time,
								 t.state,
								 t.login_fail_count,
                                 dd.domain_name,
								 ct.contact_account,
								 ct.account_type
                            FROM `account` t
                            left join website_dispatch_domain dd ON t.id = dd.account_id
							left JOIN	
									(SELECT 
                                            IFNULL(GROUP_CONCAT(ca.contact_account), ' ') as contact_account,
											IFNULL(GROUP_CONCAT(ca.account_type), ' ') AS account_type,
											ca.account_id AS account_id
                                       FROM
                                            contact_account ca
									  GROUP BY ca.account_id) as ct ON t.id = ct.account_id
                           WHERE t.state = if ({state} = 0, t.state, {state})
                             AND t.login_name like if ({loginName} = '',t.login_name ,{loginName})
                           ORDER BY t.id
                           LIMIT {page} , {size};
                          SELECT FOUND_ROWS() as cnt; ";

            return await _dbFactory.OperateAsync(async a =>
            {
                var multiple = await a.QueryMultipleAsync(tsql, new { loginName, state });
                var rows = await multiple.ReadAsync<AccountTrafficEntity>();
                var total = await multiple.ReadSingleAsync<long>();
                return (rows, total);
            });
        }
    }
}
