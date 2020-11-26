using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Phoenixnet.Extensions.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Domain.Domain
{
    public class DomainRepository : IDomainRepository
    {

        private readonly ILogger<DomainRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDbFactory _dbFactory;
        
        public DomainRepository (ILogger<DomainRepository> logger,
                                   IConfiguration configuration,
                                   IDbFactory dbFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _dbFactory = dbFactory;
        }

        #region for dispatch-domain
        public async Task<int> CheckDispatchId(long id)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"SELECT  COUNT(id) count
                                FROM  website_dispatch_domain dd
                                WHERE dd.id = @id";

                    var result = await a.QuerySingleAsync<int>(tsql, new { id });

                    _logger.LogInformation($"result{result}");
                    return result;

                });
            }

            catch(Exception ex)
            {
                _logger.LogError($"CHECK DISPATCH ID{ex.Message}");
                return 0;
            }
        }

        public async Task<long> GetDispatchDomainID(long accountid)
        {
            try
            {

                return await _dbFactory.OperateAsync(async a =>
                {

                    string tsql = @"SELECT dd.id
                                FROM website_dispatch_domain dd    
                                WHERE dd.account_id = @accountid;";

                    var result = await a.QuerySingleAsync<long>(tsql, new { accountid });

                    return result;
                });
            }

            catch(Exception ex)
            {
                _logger.LogError($"GET DISPATCH BY DOMAINID{ex.Message}");
                return 0;
            }
        }

        public async Task<(IEnumerable<DispatchEntity>rows,long total)> GetDispatchDomain(int pageoffset, int pagesize)
        {

            try
            {

                return await _dbFactory.OperateAsync(async a =>
                {

                    string totalsql = @"SELECT COUNT(*) AS total
                                    FROM website_dispatch_domain dd;";

                    string tsql = @"SELECT   
                                         dd.id as Id,dd.account_id as AccountId,dd.domain_name as DomainName,
                                         dd.cdate as Cdate,dd.ctime asCtime,dd.udate as Udate,dd.utime as Utime,
                                         dd.state as State 
                                FROM     website_dispatch_domain dd
                                ORDER BY dd.utime desc
                                LIMIT @pageoffset,@pagesize;
                                ";

                    var multiple = await a.QueryMultipleAsync(totalsql + tsql, new { pageoffset, pagesize });
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();
                    var rows = await multiple.ReadAsync<DispatchEntity>();



                    return (rows, total);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"GET DISPATCH DOMAINS ERROR:{ex}");
                return (Enumerable.Empty<DispatchEntity>(), 0);
            }
        }

        public async Task<DispatchEntity> GetDispatchDomainNameByAccount(long accountid)
        {

            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {

                    string tsql = @"SELECT dd.id as Id,dd.account_id as AccountId,dd.domain_name as DomainName,dd.state as State
                                FROM website_dispatch_domain dd
                                WHERE dd.account_id = @accountid";

                    var result = await a.QueryFirstOrDefaultAsync<DispatchEntity>(tsql, new { accountid });

                    return result;

                });
            }

            catch(Exception ex)
            {
                _logger.LogError($"GET DISPATCH BY ID{ex.Message}");
                return new DispatchEntity { };
            }

        }

        public async Task<int> CreateDispatchData(DispatchEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
               {

                   string tsql = @"INSERT INTO website_dispatch_domain (id,account_id,domain_name,cdate,ctime,udate,utime,state) 
                                VALUES(@Id,@AccountId,@DomainName,@Cdate,@Ctime,@Udate,@Utime,@State);";

                   var result = await a.ExecuteAsync(tsql, entity);
                   return result;

               });
            }

            catch(Exception ex)
            {
                _logger.LogError($"CREATE DISPATCHDOMAIN ERROR:{ex.Message}");
                return 0;
            }
        }


        public async Task<int> CheckAccountId(long accountid)
        {
            try
            {

                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"SELECT COUNT(a.id) count
                                FROM mwm_all.account a
                                WHERE a.id = @accountid;";
                    var result = await a.QuerySingleAsync(tsql, new { accountid });

                    return result;
                });
            }

            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return 0;
            }


        }
        #endregion


        #region for video-domain
        public async Task<int> CreateDomain(DomainEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"INSERT INTO video_domain ( id,domain_name,cdate,ctime,udate,utime,state)
                                VALUES(@Id,@DomainName,@Cdate,@Ctime,@Udate,@Utime,@State)";

                    var result = await a.ExecuteAsync(tsql, entity);

                    return result;
                });
            }
            catch(Exception ex)
            {
                _logger.LogError($"CREATE DOMAIN ERROR:{ex.Message}");
                return 0;
            }
        }

        public async Task<IEnumerable<DomainEntity>> GetDomains()
        {
            return await _dbFactory.OperateAsync(async a => 
            {
                string tsql = @"SELECT vd.id as Id ,vd.domain_name as DomainName,vd.cdate ,vd.ctime,
                                       vd.udate,vd.utime,vd.state 
                                FROM   video_domain vd";

                try
                {
                    return await a.QueryAsync<DomainEntity>(tsql);
                    
                }
                catch(Exception ex)
                {
                    _logger.LogInformation($"EXCEPTION:{ex.ToString()}");
                    return Enumerable.Empty<DomainEntity>();
                }

                
            });
        }
        public async Task<DomainEntity> GetDomainById(long id)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"SELECT vd.id as Id,vd.domain_name as DomainName ,vd.state as State
                                FROM video_domain vd
                                WHERE vd.id =@id";

                    var result = await a.QueryFirstOrDefaultAsync<DomainEntity>(tsql, new { id });

                    return result;
                });
            }

            catch(Exception ex)
            {
                _logger.LogError($"GET DOMAIN BY ID ERROE:{ex.Message}");
                return new DomainEntity { };
            }

        }


        #region TODO
        public async Task<int> UpdateDomain(List<DomainEntity> entity)
        {
            int result = 0;
            try
            { 
                    return await _dbFactory.OperateAsync(async a =>
                    {
                        using (var trans = a.BeginTransaction())
                        {
                            string tsql = $@"INSERT INTO video_domain (id,domain_name,cdate,ctime,udate,utime,state)
                                             VALUES(@Id,@DomainName,@Cdate,@Ctime,@Udate,@Utime,@State)       
                                      ON DUPLICATE KEY UPDATE 
                                      domain_name=IF(@DomainName='',domain_name,@DomainName),
                                      cdate=IF(@Cdate=0,cdate,@Cdate),
                                      ctime=IF(@Ctime=0,ctime,@Ctime),
                                      udate=IF(@Udate=0,udate,@Udate),
                                      utime=IF(@Utime=0,utime,@Utime),
                                      state=IF(@State=0,state,@State);";
                            result = await a.ExecuteAsync(tsql, entity, trans);
                            trans.Commit();
                        }
                        return result;
                    }); 
               
               
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return 0;
            }
        }
        #endregion TODO




        public async Task<int> DeleteDomain(long id)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"UPDATE video_domain vd 
                                SET    vd.state = 99
                                WHERE  vd.id = @id";

                    var result = await a.ExecuteAsync(tsql, new { id });

                    return result;
                });
            }

            catch(Exception ex)
            {
                _logger.LogError($"DELETE DOMAIN ERROR:{ex.Message}");
                return 0;
            }
        }
        public async Task<int> CheckDomainId(long id)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"SELECT  COUNT(id) count
                                FROM  video_domain vd
                                WHERE vd.id = @id";

                    var result = await a.QuerySingleAsync<int>(tsql, new { id });

                    _logger.LogInformation($"result{result}");
                    return result;

                });
            }
            catch(Exception ex)
            {
                _logger.LogError($"CHECK DOMAINID ERROR{ex.Message}");
                return 0;
            }
        }

        #endregion



        #region for websitedomain             
        public async Task<int> CheckWebsiteId(long id)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {
                    string tsql = @"SELECT  COUNT(id) count
                                FROM  website_dispatch_domain dd
                                WHERE dd.id = @id";

                    var result = await a.QuerySingleAsync<int>(tsql, new { id });

                    _logger.LogInformation($"result : {result}");
                    return result;

                });
            }
            catch(Exception ex)
            {
                _logger.LogError($"CHECK ID ERROR:{ex.Message}");
                return 0;
            }
        }
        
        public async Task<int> CreateWebsiteDomain(WebSiteDomainEntity entity)
        {
            try
            {

                return await _dbFactory.OperateAsync(async a =>

                {
                    string tsql = @"INSERT INTO website_domain (id,account_id,dispatch_domain_id,template_id,public_domain,website_name,website_desc,
                                                            keyword,cdate,ctime,udate,utime,state)
                                VALUES(@Id,@AccountId,@DispatchDomainId,@TemplateId,@PubDomain,@WebsiteName,@WebsiteDesc,@Keyword,
                                                            @Cdate,@Ctime,@Udate,@Utime,@State)";

                    var result = await a.ExecuteAsync(tsql, entity);

                    return result;
                });
            }
            catch(Exception ex)
            {
                _logger.LogError($"CREATE WEBSITE ERROR:{ex.Message}");
                return 0;
            }

        }
        
        public async Task<(IEnumerable<WebSiteDomainEntity> rows, long total)> GetWebsiteDomainById(long accountid, int pageoffset, int pagesize)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {

                    string totalsql = @"SELECT COUNT(*) AS total
                                        FROM website_domain wd
                                        WHERE  wd.account_id =@accountid;";
                                          
                    string tsql = @"SELECT wd.id as Id,wd.account_id as AccountId,wd.dispatch_domain_id as DispatchDomainId,wd.template_id as TemplateId,wd.public_domain as PubDomain,
                                      wd.website_name as WebsiteName,wd.website_desc as WebsiteDesc,wd.keyword as Keyword, wd.cdate as Cdate,
                                      wd.ctime as Ctime,wd.udate as Udate,wd.utime as Utime,wd.state as State
                                    FROM website_domain wd
                                    WHERE  wd.account_id =@accountid
                                    ORDER BY from_unixtime(wd.utime,'%Y%m%d') desc
                                    LIMIT @pageoffset ,@pagesize;";

                    var multiple = await a.QueryMultipleAsync(totalsql + tsql, new { accountid,pageoffset, pagesize });
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();
                    var rows = await multiple.ReadAsync<WebSiteDomainEntity>();


                    return (rows, total);

                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"GET BY ID ERROR:{ex}");
                return (Enumerable.Empty<WebSiteDomainEntity>(), 0);
            }

        }
      
        public async Task<(IEnumerable<WebSiteDomainEntity> rows, long total)> GetWebsiteDomains(int pageoffset, int pagesize)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a => {

                string totalsql = @"SELECT COUNT(*) AS total
                                    FROM website_domain wd
                                    WHERE wd.state =1;";
                
                string tsql = @"SELECT wd.id as Id,wd.account_id as AccountId,wd.dispatch_domain_id as DispatchDomainId,template_id as TemplateId,wd.public_domain as PubDomain,
                                       wd.website_name as WebsiteName,wd.website_desc as WebsiteDesc,wd.keyword as Keyword, wd.cdate as Cdate,
                                       wd.ctime as Ctime,wd.udate as Udate,wd.utime as Utime,wd.state as State
                               FROM website_domain wd
                               WHERE wd.state =1
                               ORDER BY from_unixtime(wd.utime,'%Y%m%d') desc
                               LIMIT @pageoffset ,@pagesize;";
                
                    var multiple = await a.QueryMultipleAsync(tsql + totalsql, new { pageoffset, pagesize });
                    var rows = await multiple.ReadAsync<WebSiteDomainEntity>();
                    var total = await multiple.ReadFirstOrDefaultAsync<long>();

                    return (rows, total);
                    
            });
        }

            catch (Exception ex)
            {
                _logger.LogError($"GET ERROR:{ex}");
                return (Enumerable.Empty<WebSiteDomainEntity>(), 0);
            }
        }
        
        public async Task<int> DeleteWebsiteDomain(WebSiteDomainEntity entity)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {

                    string tsql = @"UPDATE website_domain wd
                                SET wd.state = 99
                                WHERE wd.id = @Id";

                    var result = await a.ExecuteAsync(tsql, entity);

                    return result;
                });
            }

            catch(Exception ex)
            {
                _logger.LogError($"DELETE WEBSITE ERROR:{ex.Message}");
                return 0;
            }

        }
        
        public async Task<int> UpdateWebsiteDomain(WebSiteDomainEntity entity)
        {
            try
            {

                return await _dbFactory.OperateAsync(async a =>
                {

                    string tsql = @"UPDATE website_domain wd
                                SET   wd.dispatch_domain_id = IF(@DispatchDomainId=0,wd.dispatch_domain_id,@DispatchDomainId),
                                      wd.template_id =IF(@TemplateId=0,wd.template_id,@TemplateId),
                                      wd.public_domain =IF(@PubDomain='',wd.public_domain,@PubDomain),
                                      wd.website_name=IF(@WebsiteName='',wd.website_name,@WebsiteName),
                                      wd.website_desc=IF(@WebsiteDesc='',wd.website_desc,@WebsiteDesc),
                                      wd.keyword=IF(@Keyword='',wd.keyword,@Keyword),
                                      wd.cdate=IF(@Cdate=0,wd.cdate,@Cdate),
                                      wd.cdate=IF(@Cdate=0,wd.cdate,@Cdate),
                                      wd.udate=IF(@Udate=0,wd.udate,@Udate),
                                      wd.utime=IF(@Utime=0,wd.utime,@Utime),
                                      wd.state=IF(@State=0,wd.state,@State)
                                WHERE wd.account_id = @AccountId AND wd.id =@Id";

                    var result = await a.ExecuteAsync(tsql, entity);

                    return result;
                });
            }

            catch(Exception ex)
            {
                _logger.LogError($"UPDATE WEBSITE ERROR:{ex.Message}");
                return 0;
            }
        }

        public async  Task<IEnumerable<WebSiteDomainEntity>> GetWebdomainSettingByAccount(long accountid)
        {
            try
            {
                return await _dbFactory.OperateAsync(async a =>
                {  
                    string tsql = @"SELECT wd.id as Id,wd.account_id as AccountId,wd.dispatch_domain_id as DispatchDomainId,wd.template_id as TemplateId,wd.public_domain as PubDomain,
                                      wd.website_name as WebsiteName,wd.website_desc as WebsiteDesc,wd.keyword as Keyword, wd.cdate as Cdate,
                                      wd.ctime as Ctime,wd.udate as Udate,wd.utime as Utime,wd.state as State
                                    FROM website_domain wd
                                    WHERE  wd.account_id =@accountid
                                    ORDER BY from_unixtime(wd.utime,'%Y%m%d') desc;";

                    var result = await a.QueryAsync<WebSiteDomainEntity>(tsql, new { accountid});
                    return result;

                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"GET BY ID ERROR:{ex}");
                throw ex;
            }

        }
        #endregion
    }
}
