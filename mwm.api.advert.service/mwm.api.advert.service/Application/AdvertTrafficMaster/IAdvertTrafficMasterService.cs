using System.Collections.Generic;
using System.Threading.Tasks;
using MWM.API.Advert.Service.Application.AdvertTrafficMaster.Contract;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Advert.Service.Application.AdvertTrafficMaster
{
    /// <summary>
    /// 流量主後台-我的廣告
    /// </summary>
    public interface IAdvertTrafficMasterService
    {
        #region 上方橫幅廣告

        /// <summary>
        ///     新增上方橫幅廣告
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> Add_Top_Banner_Record(AddTopBannerRequest request,int account_id);

        /// <summary>
        ///     更新上方橫幅廣告
        /// </summary>
        /// <param name="request"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        Task<BasicResponse> Update_Top_Banner_Record(UpdateTopBannerRequest request,int account_id);

        /// <summary>
        ///     刪除上方橫幅廣告
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        Task<BasicResponse> del_Top_Banner_Record(int id,int account_id);


        /// <summary>
        ///     取得上方橫幅廣告列表
        /// </summary>
        /// <returns></returns>
        Task<IPagingResult<TopBannerResponse>> Get_Top_Banner_Record(int index, int size,int account_id);

        #endregion
        #region 漂浮廣告

        /// <summary>
        /// 取得漂浮廣告列表
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        Task<BasicResponse<IEnumerable<AdriftResponse>>> GetAdriftList(int account_id);

        /// <summary>
        ///     更新漂浮廣告
        /// </summary>
        /// <param name="request"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        Task<BasicResponse> UpdateAdrift(AdriftRequest request,int account_id);

        #endregion
        #region 全站彈跳視窗

        /// <summary>
        ///     取得全站彈跳視窗
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        Task<BasicResponse<AllSitePopUpResponse>> GetAllSitePopUp(int account_id);

        /// <summary>
        ///     更新全站彈跳視窗
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> UpdateAllSitePopUp(AllSitePopUpdateRequest request,int account_id);

        #endregion
        #region 片頭彈窗廣告

        /// <summary>
        ///     取得片頭彈窗廣告
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        Task<BasicResponse<VideoPopUpResponse>> GetVideoPopUp(int account_id);

        /// <summary>
        ///     更新片頭彈窗廣告
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> UpdateVideoPopUp(VideoPopUpUpdateRequest request,int account_id);

        #endregion
        #region 影片banner

        /// <summary>
        ///     新增影片banner
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> Add_Video_Banner_Record(AddVideoBannerRequest request,int account_id);

        /// <summary>
        ///     更新影片banner
        /// </summary>
        /// <param name="request"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        Task<BasicResponse> Update_Video_Banner_Record(UpdateVideoBannerRequest request,int account_id);

        /// <summary>
        ///     刪除影片banner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        Task<BasicResponse> del_Video_Banner_Record(int id,int account_id);


        /// <summary>
        ///     取得影片banner列表
        /// </summary>
        /// <returns></returns>
        Task<IPagingResult<AddVideoBannerResponse>> Get_Video_Banner_Record(int index, int size,int account_id);

        #endregion
    }
}