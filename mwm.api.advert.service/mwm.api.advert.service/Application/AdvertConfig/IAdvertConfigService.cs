using System.Threading.Tasks;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Advert.Service.Application.AdvertConfig
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAdvertConfigService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse<bool>> sponsor_isEnable();
        
        Task<BasicResponse> Update(bool enable);
        
        
    }
}