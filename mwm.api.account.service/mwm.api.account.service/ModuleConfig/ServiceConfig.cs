using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MWM.API.Account.Service.Application.Bulletin;
using MWM.API.Account.Service.Application.Common;
using MWM.API.Account.Service.Application.Message;
using MWM.API.Account.Service.Domain.Account;
using MWM.API.Account.Service.Domain.Message;
using MWM.API.Account.Service.Domain.Bulletin;
using MWM.API.Account.Service.Domain.SyncDefaultAdvert;
using Phoenixnet.Extensions;
using MWM.API.Account.Service.Application.Permission;
using MWM.API.Account.Service.Domain.Permission;
using MWM.API.Account.Service.Application.AccountTraffic;
using MWM.API.Account.Service.Domain.AccountTraffic;
using MWM.API.Account.Service.Application.AccountManager;
using MWM.API.Account.Service.Domain.AccountManager;
//using MWM.API.Account.Service.Application.AccountHistoryService;
//using MWM.API.Account.Service.Domain.AccountHistory;

namespace MWM.API.Account.Service.ModuleConfig
{
    public class RepositoryConfig : IAbstractModule
    {
        public void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IVerifyRequest, VerifyRequest>();

            services.AddTransient<IAccountTrafficService, AccountTrafficService>()
                    .AddTransient<ISyncDefaultAdvert,SyncDefaultAdvert>()
                    .AddTransient<IAccountRepository, AccountRepository>();

            services.AddTransient<IBulletinService, BulletinService>()
                    .AddTransient<IBulletinRepository, BulletinRepository>();

            services.AddTransient<IMessageService, MessageService>()
                    .AddTransient<IMessageRepository, MessageRepository>();

            services.AddTransient<IPermissionService, PermissionService>()
                    .AddTransient<IPermissionRepository, PermissionRepository>();

            services.AddTransient<IAccountManagerService, AccountManagerService>()
                    .AddTransient<IAccountManagerRepository, AccountManagerRepository>();

            //services.AddTransient<IAccountHistoryService,AccountHistoryService>()
            //       .AddTransient<IAccountHistoryRepository, AccountHistoryRepository>();
        }
    }
}