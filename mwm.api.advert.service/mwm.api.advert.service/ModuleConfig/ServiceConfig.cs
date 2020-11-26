using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MWM.API.Advert.Service.Application.AdvertConfig;
using MWM.API.Advert.Service.Application.AdvertTrafficMaster;
using MWM.API.Advert.Service.Application.Receivable;
using MWM.API.Advert.Service.Domain.Receivable;
using MWM.API.Advert.Service.Application.Common;
using MWM.API.Advert.Service.Application.DefaultAdvert;
using MWM.API.Advert.Service.Application.SponsorAdvertTrafficMaster;
using MWM.API.Advert.Service.Application.VideoAdvert.video;
using MWM.API.Advert.Service.Domain.AdvertConfig;
using MWM.API.Advert.Service.Domain.AdvertTrafficMaster;
using MWM.API.Advert.Service.Domain.DefaultAdvert;
using MWM.API.Advert.Service.Domain.SponsorAdvert;
using MWM.API.Advert.Service.Domain.SponsorAdvertTrafficMaster;
using MWM.API.Advert.Service.Domain.VideoAdvert;
using MWM.API.Advert.Service.Domain.VideoAdvert.video;
using Phoenixnet.Extensions;
using ISponsorAdvertService = MWM.API.Advert.Service.Application.SponsorAdvert.ISponsorAdvertService;
using SponsorAdvertService = MWM.API.Advert.Service.Application.SponsorAdvert.SponsorAdvertService;

namespace MWM.API.Advert.Service.ModuleConfig
{
    /// <summary>
    /// 
    /// </summary>
    public class RepositoryConfig : IAbstractModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IVerifyRequest, VerifyRequest>();

            services.AddTransient<IReceivablePasswordService, ReceivablePasswordService>()
                    .AddTransient<IReceivableBankService, ReceivableBankService>()
                    .AddTransient<IReceivableRecordService, ReceivableRecordService>();

            services.AddTransient<IReceivableBankRepository, ReceivableBankRepository>()
                    .AddTransient<IReceivablePasswordRepository, ReceivablePasswordRepository>()
                    .AddTransient<IReceivableRecordRepository, ReceivableRecordRepository>();
            
            //官網後台-預設廣告
            services.AddTransient<IDefaultAdvertRepository, DefaultAdvertRepository>()
                    .AddTransient<IDefaultAdvertService, DefaultAdvertService>();

            //官網後台-影片區域
            services.AddTransient<IVideoAdvertRepository, VideoAdvertRepository>()
                .AddTransient<IVideoAdvertVideoService, VideoAdvertVideoService>();
            
            //官網後台-全民贊助廣告
            services
                .AddTransient<ISponsorAdvertRepository, SponsorAdvertRepository>()
                .AddTransient<ISponsorAdvertService, SponsorAdvertService>();
   
            //官網後台-全民贊助廣告設定
            services.AddTransient<IAdvertConfigRepository, AdvertConfigRepository>()
                .AddTransient<IAdvertConfigService, AdvertConfigService>();
            
            
            //流量主後台-預設廣告
            services.AddTransient<IAdvertTrafficMasterRepository, AdvertTrafficMasterRepository>()
                .AddTransient<IAdvertTrafficMasterService,AdvertTrafficMasterService>();
            //流量主後台-全民贊助廣告
            services.AddTransient<ISponsorAdvertTrafficMasterRepository, SponsorAdvertTrafficMasterRepository>()
                .AddTransient<ISponsorAdvertTrafficMasterService, SponsorAdvertTrafficMasterService>();


        }
    }
}