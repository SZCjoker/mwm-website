using System.Threading.Tasks;
using MWM.API.Advert.Service.Application.SponsorAdvert.Contract;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Advert.Service.Application.SponsorAdvert
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISponsorAdvertService
    {
        /// <summary>
        ///新增平面廣告
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> Add_Print_Record(SponsorAdvertRequest request);
        
        
        /// <summary>
        /// 更新平面廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BasicResponse> Update_Print_Record(UpdateSponsorAdvertRequest request);
        
        /// <summary>
        /// 刪除平面廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BasicResponse> Del_Print_Record(int id);
        
        
        /// <summary>
        /// 取得平面廣告列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<IPagingResult<SponsorAdvertResponse>> Get_Print_Record(int index, int size);
    }
}