using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Phoenixnet.Extensions.Data.MySql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Template
{
    public class TemplateRepository : ITemplateRepository
    {

        private readonly ILogger<TemplateRepository> _logger;
        private readonly IDbFactory _dbfctory;
        private readonly IConfiguration _configuration;
        
        
        public TemplateRepository(ILogger<TemplateRepository> logger,
                                    IDbFactory dbFactory,
                                    IConfiguration configuration)
        {
            _logger = logger;
            _dbfctory = dbFactory;
            _configuration = configuration;
        }


        
        
        public async Task<int> CheckId(int id)
        {
            return await _dbfctory.OperateAsync(async a => {

                string tsql = @"SELECT COUNT(id) count
                                FROM website_template wt 
                                WHERE wt.id =@id ";
                var result = await a.ExecuteAsync(tsql,new { id});

                return result;
            });
        }

        public async Task<int> CreateTemplate(TemplateEntity entity)
        {
            return await _dbfctory.OperateAsync(async a => {

                string tsql = @"INSERT INTO website_template(id,img,`name`,`desc`,advert_amount,cdate,ctime,udate,utime,state)
                                VALUES(@Id,@Img,@Name,@Desc,@AdvertAmount,@Cdate,@Ctime,@Udate,@Utime,@State)";
                var result = await a.ExecuteAsync(tsql,entity);

                return result;
            });
        }

        public async Task<int> DeleteTemplate(TemplateEntity entity)
        {
            return await _dbfctory.OperateAsync(async a => { 
            
            string tsql = @"UPDATE website_template wt
                            SET wt.state = 2 
                            WHERE wt.id =@Id";
                var result = await a.ExecuteAsync(tsql, entity);
                return result;
            });
        }

        public async Task<TemplateEntity> GetTemplateById(int id)
        {
            return await _dbfctory.OperateAsync(async a=> {
                string tsql = @"SELECT wt.img as Img,wt.`name` as `Name`,wt.`desc` as `Desc`,wt.advert_amount as AdvertAmount,
                                       wt.cdate as Cdate,wt.ctime as Ctime,wt.udate as Udate, wt.utime as Utime,wt.state as State
                                FROM website_template wt
                                WHERE wt.id = @id";

                var result = await a.QuerySingleOrDefaultAsync<TemplateEntity>(tsql, new { id });
                return result;
            });
        }

        public async Task<IEnumerable<TemplateEntity>> GetTemplates()
        {
            return await _dbfctory.OperateAsync(async a => {
                string tsql = @"SELECT wt.id as Id,wt.img as Img,wt.`name` as `Name`,wt.`desc` as `Desc`,wt.advert_amount as AdvertAmount,
                                       wt.cdate as Cdate,wt.ctime as Ctime,wt.udate as Udate, wt.utime as Utime, wt.state as State
                                FROM   mwm_all.website_template wt";

                var result = await a.QueryAsync<TemplateEntity>(tsql);
                return result;
            });
        }

        public async Task<IEnumerable<TemplateEntity>> QuerybyCondition(string queryStr)
        {
            try
            {
                return await _dbfctory.OperateAsync(async a =>
                {
                   
                    string tsql = $@"SELECT   wt.id as Id,wt.img as Img,wt.`name` as `Name`,
                                          wt.`desc` as `Desc`,wt.cdate as Cdate,
                                          wt.ctime as Ctime,wt.state as State
                                 FROM website_template wt                                
                                 WHERE 1=1{queryStr} 
                                 ORDER BY from_unixtime(wt.ctime,'%Y%m%d') desc;";

                    var result = await a.QueryAsync<TemplateEntity>(tsql);

                    return result;

                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"QUERY ERROR :{ex.Message},{queryStr}");
                throw ex;
            }


        }

        public async Task<int> UpdateTemplate(TemplateEntity entity)
        {
            return await _dbfctory.OperateAsync(async a => {
                string tsql = @"UPDATE website_template wt
                                SET wt.img = IF(@Img='',wt.img,@Img),
                                    wt.`name`=IF(@`Name`='',wt.`name`,@Name),
                                    wt.`desc` = IF(@`Desc`='',wt.`desc`,@`Desc`),
                                    wt.advert_amount = IF(@AdvertAmount=0,wt.advert_amount,@AdvertAmount), 
                                    wt.cdate = IF(@Cdate = 0, wt.cdate, @Cdate), 
                                    wt.ctime = IF(@Ctime = 0, wt.ctime, @Ctime),
                                    wt.udate = IF(@Udate = 0, wt.udate, @Udate),
                                    wt.utime = IF(@Utime = 0, wt.utime, @Utime),
                                    wt.state = IF(@State = 0, wt.state, @State)
                                WHERE wt.id = @Id";
                var result = await a.ExecuteAsync(tsql,entity);
                return result;
            });
        }
    }
}
