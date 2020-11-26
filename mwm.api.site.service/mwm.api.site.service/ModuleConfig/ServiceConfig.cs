using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MWM.API.Site.Service.Application.AccountHistory;
using MWM.API.Site.Service.Application.AdvertReport;
using MWM.API.Site.Service.Application.Code;
using MWM.API.Site.Service.Application.Common;
using MWM.API.Site.Service.Application.Domain;
using MWM.API.Site.Service.Application.Link;
using MWM.API.Site.Service.Application.ReferReport;
using MWM.API.Site.Service.Application.Site;
using MWM.API.Site.Service.Application.Template;
using MWM.API.Site.Service.Domain;
using MWM.API.Site.Service.Domain.AccountHistory;
using MWM.API.Site.Service.Domain.AdvertReport;
using MWM.API.Site.Service.Domain.Code;
using MWM.API.Site.Service.Domain.Domain;
using MWM.API.Site.Service.Domain.Link;
using MWM.API.Site.Service.Domain.ReferReport;
using MWM.API.Site.Service.Domain.Site;
using MWM.API.Site.Service.Domain.Template;
using MWM.Extensions.Authentication.JWT;
using Phoenixnet.Extensions;
using static MWM.API.Site.Service.Application.Domain.SyncDomain;



namespace MWM.API.Site.Service.ModuleConfig
{
    public class RepositoryConfig : IAbstractModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IVerifyRequest, VerifyRequest>();

            services.AddTransient<IGetJwtTokenInfoService, GetJwtTokenInfoService>();             

            services.AddTransient<ISiteService, SiteService>()
                    .AddTransient<ISiteRepository, SiteRepository>();

            services.AddTransient<ISyncDomain, SyncDomain>()
                    .AddTransient<ISync, DomainAsync>();

            services.AddTransient<IDomainService, DomainService>()
                    .AddTransient<IDomainRepository, DomainRepository>();

            services.AddTransient<ITemplateService,TemplateService>()
                    .AddTransient<ITemplateRepository, TemplateRepository>();

            services.AddTransient<ILinkService, LinkService>()
                    .AddTransient<ILinkRepository, LinkRepository>();
            
            services.AddTransient<ICodeService, CodeService>()
                    .AddTransient<ICodeRepository, CodeRepository>();

            services.AddTransient<IAccountHistoryService, AccountHistoryService>()
                    .AddTransient<IAccountHistoryRepository, AccountHistoryRepository>(); 
            
            services.AddTransient<IReferReportRepository, ReferReportRepository>()
                    .AddTransient<IReferReportService,ReferReportService>();
         
            services.AddTransient<IAdvertReportRepository, AdvertReportRepository>()
                    .AddTransient<IAdvertReportService,AdvertReportService>();

        }
    }
}