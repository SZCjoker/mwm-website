﻿using Microsoft.Extensions.Logging;
using Phoenixnet.Extensions.Data.MySql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.AccountHistory
{
    public class AccountHistoryRepository:IAccountHistoryRepository
    {

        private readonly ILogger<AccountHistoryRepository> _logger;
        private readonly IDbFactory _dbFactory;

        public AccountHistoryRepository(ILogger<AccountHistoryRepository> logger, IDbFactory dbFactory)
        {
            _logger = logger;
            _dbFactory = dbFactory;
        }



        public async Task<(IEnumerable<AccountHistoryEntity> rows, long total)> GetAccountHistoryByCondition(QueryAllEntity entity, string queryStr)
        {
            _logger.LogInformation($"query condition--{entity.PageOffset}+{entity.PageSize}+{entity.BeginDate}+{entity.EndDate}");
            return await _dbFactory.OperateAsync(async a =>
            {
                string tsql = $@"SELECT acc.login_name as LoginName,ah.account_id as AccountId,ah.update_data as UpdateData,
                                        ah.`action`as Action,ah.category as Category,ah.before_data as BeforeData,
                                        ah.after_data as AfterData,ah.ip as Ip,ah.udate as Udate,ah.utime as Utime
							    FROM    account acc
                                JOIN    account_history ah on acc.id = ah.account_id
                                WHERE 1=1
                                {queryStr} 
                                ORDER BY  ah.utime desc
                                LIMIT @PageOffset ,@PageSize;";

                string totalsql = $@"SELECT COUNT(*) AS total
                                    FROM account_history ah
                                    WHERE 1 = 1
                                    {queryStr};";

                var multiple = await a.QueryMultipleAsync(tsql + totalsql, entity);
                var rows = await multiple.ReadAsync<AccountHistoryEntity>();
                var total = await multiple.ReadFirstOrDefaultAsync<long>();



                _logger.LogInformation($"querystr--{queryStr}+result{rows.Count()}");


                return (rows, total);

            });
        }

        public async Task<IEnumerable<AccountHistoryEntity>> GetAllHistory()
        {
            return await _dbFactory.OperateAsync(async a =>
            {

                string tsql = @"SELECT ah.account_id as AccountId,a.login_name as LoginName,count(*) as Count
                                FROM   account_history ah
                                JOIN   account a on ah.account_id = a.id
                                GROUP  BY account_id";
                try
                {
                    return await a.QueryAsync<AccountHistoryEntity>(tsql);
                }

                catch (Exception ex)
                {
                    _logger.LogError($"ex:{ex}");
                    return Enumerable.Empty<AccountHistoryEntity>();
                }


            });


        }

        public async Task<int> SaveAccountHistory(AccountHistoryEntity record)
        {
            return await _dbFactory.OperateAsync(async a =>
            {

                string tsql = @"INSERT INTO account_history(id,account_id,category,action,update_data,before_data,after_data,ip,udate,utime)
                                VALUES               (@Id,@AccountId,@Category,@Action,@UpdateData,@BeforeData,@AfterData,@Ip,@Udate,@Utime);";

                var result = await a.ExecuteAsync(tsql, record);

                return result;
            });
        }

    }
}
