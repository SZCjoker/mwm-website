using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.AdvertConfig
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAdvertConfigRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<bool> Sponsor_isEnable();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<int> Update_Record(bool enable);
    }
}