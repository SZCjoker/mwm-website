using System.Threading.Tasks;
using MWM.API.Advert.Service.Domain.AdvertConfig;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Advert.Service.Application.AdvertConfig
{
    /// <summary>
    /// 
    /// </summary>
    public class AdvertConfigService:IAdvertConfigService
    {
        private readonly IAdvertConfigRepository _advertConfigRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="advertConfigRepository"></param>
        public AdvertConfigService(IAdvertConfigRepository advertConfigRepository)
        {
            _advertConfigRepository = advertConfigRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public async Task<BasicResponse<bool>> sponsor_isEnable()
        {
            var result = await _advertConfigRepository.Sponsor_isEnable();

            return StateCodeHandler.ForBool(true,result);
        }

        public async Task<BasicResponse> Update(bool enable)
        {
            var result =await _advertConfigRepository.Update_Record(enable);
            return StateCodeHandler.ForCount(result);
        }
    }
}