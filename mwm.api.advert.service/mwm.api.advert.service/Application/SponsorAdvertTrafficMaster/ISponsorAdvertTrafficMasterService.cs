using System.Threading.Tasks;
using MWM.API.Advert.Service.Application.SponsorAdvertTrafficMaster.Contract;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Advert.Service.Application.SponsorAdvertTrafficMaster
{
    /// <summary>
    /// 流量主-全民贊助廣告
    /// </summary>
    public interface ISponsorAdvertTrafficMasterService
    {
        /// <summary>
        ///新增全民贊助廣告
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> Add_Print_Record(int account_id,SponsorTrafficMasterRequest trafficMasterRequest);


        /// <summary>
        /// 更新全民贊助廣告
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="trafficMasterRequest"></param>
        /// <returns></returns>
        Task<BasicResponse> Update_Print_Record(int account_id,UpdateSponsorTrafficMasterRequest trafficMasterRequest);
        
        /// <summary>
        /// 刪除全民贊助廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BasicResponse> Del_Print_Record(int id);


        /// <summary>
        /// 取得全民贊助廣告列表
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<IPagingResult<SponsorTrafficMasterResponse>> Get_Print_Record(int account_id,int index, int size);
    }
}