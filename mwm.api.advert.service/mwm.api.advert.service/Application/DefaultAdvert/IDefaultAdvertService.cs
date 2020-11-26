using System.Collections.Generic;
using System.Threading.Tasks;
using MWM.API.Advert.Service.Application.DefaultAdvert.Contract;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Advert.Service.Application.DefaultAdvert
{
    /// <summary>
    /// 預設廣告介面
    /// </summary>
    public interface IDefaultAdvertService
    {
        #region 上方橫幅廣告

        /// <summary>
        ///     新增上方橫幅廣告
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> Add_Top_Banner_Record(AddTopBannerRequest request);

        /// <summary>
        ///     更新上方橫幅廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BasicResponse> Update_Top_Banner_Record(UpdateTopBannerRequest request);

        /// <summary>
        ///     刪除上方橫幅廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BasicResponse> del_Top_Banner_Record(int id);


        /// <summary>
        ///     取得上方橫幅廣告列表
        /// </summary>
        /// <returns></returns>
        Task<IPagingResult<TopBannerResponse>> Get_Top_Banner_Record(int index, int size);

        #endregion

        #region 漂浮廣告

        /// <summary>
        ///     取得漂浮廣告列表
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse<IEnumerable<AdriftResponse>>> GetAdriftList();

        /// <summary>
        ///     更新漂浮廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BasicResponse> UpdateAdrift(AdriftRequest request);

        #endregion

        #region 全站彈跳視窗

        /// <summary>
        ///     取得全站彈跳視窗
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse<AllSitePopUpResponse>> GetAllSitePopUp();

        /// <summary>
        ///     更新全站彈跳視窗
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> UpdateAllSitePopUp(AllSitePopUpdateRequest request);

        #endregion

        #region 片頭彈窗廣告

        /// <summary>
        ///     取得片頭彈窗廣告
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse<VideoPopUpResponse>> GetVideoPopUp();

        /// <summary>
        ///     更新片頭彈窗廣告
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> UpdateVideoPopUp(VideoPopUpUpdateRequest request);

        #endregion

        #region 影片banner

        /// <summary>
        ///     新增影片banner
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> Add_Video_Banner_Record(AddVideoBannerRequest request);

        /// <summary>
        ///     更新影片banner
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BasicResponse> Update_Video_Banner_Record(UpdateVideoBannerRequest request);

        /// <summary>
        ///     刪除影片banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BasicResponse> del_Video_Banner_Record(int id);


        /// <summary>
        ///     取得影片banner列表
        /// </summary>
        /// <returns></returns>
        Task<IPagingResult<AddVideoBannerResponse>> Get_Video_Banner_Record(int index, int size);

        #endregion
    }
}