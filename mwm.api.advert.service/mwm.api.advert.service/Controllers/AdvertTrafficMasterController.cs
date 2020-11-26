using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MWM.API.Advert.Service.Application.AdvertConfig;
using MWM.API.Advert.Service.Application.AdvertTrafficMaster;
using MWM.API.Advert.Service.Application.AdvertTrafficMaster.Contract;
using MWM.API.Advert.Service.Application.SponsorAdvert;
using MWM.API.Advert.Service.Application.SponsorAdvertTrafficMaster;
using MWM.API.Advert.Service.Application.SponsorAdvertTrafficMaster.Contract;
using MWM.Extensions.Authentication.JWT;

namespace MWM.API.Advert.Service.Controllers
{
    /// <summary>
    ///     流量主後台-我的廣告操作
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertTrafficMasterController : ControllerBase
    {
        private readonly IAdvertConfigService _advertConfigService;
        private readonly IAdvertTrafficMasterService _advertTrafficMasterService;
        private readonly IGetJwtTokenInfoService _getJwtTokenInfo;
        private readonly ISponsorAdvertService _sponsorAdvertService;
        private readonly ISponsorAdvertTrafficMasterService _sponsorAdvertTraffic;
    


        /// <summary>
        /// </summary>
        /// <param name="advertTrafficMasterService"></param>
        /// <param name="getJwtTokenInfo"></param>
        /// <param name="sponsorAdvertTrafficMasterService"></param>
        /// <param name="advertConfigService"></param>
        /// <param name="sponsorAdvertService"></param>
        /// <param name="sponsorAdvertService1"></param>
        public AdvertTrafficMasterController(IAdvertTrafficMasterService advertTrafficMasterService, IGetJwtTokenInfoService getJwtTokenInfo, ISponsorAdvertTrafficMasterService sponsorAdvertTrafficMasterService, IAdvertConfigService advertConfigService, ISponsorAdvertTrafficMasterService sponsorAdvertService, ISponsorAdvertService sponsorAdvertService1)
        {
            _advertTrafficMasterService = advertTrafficMasterService;
            _getJwtTokenInfo = getJwtTokenInfo;
            _advertConfigService = advertConfigService ?? throw new ArgumentNullException(nameof(advertConfigService));
            _sponsorAdvertTraffic = sponsorAdvertService;
            _sponsorAdvertService = sponsorAdvertService1;
        }

        #region 漂浮廣告(Type=0)

        /// <summary>流量主後台-我的廣告-取得漂浮廣告列表</summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("adrift")]
        public async Task<IActionResult> Get_Adrift()
        {
            var result = await _advertTrafficMasterService.GetAdriftList(_getJwtTokenInfo.UserId);
            return Ok(result);
        }

        /// <summary> 流量主後台-我的廣告-更新漂浮廣吿 </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("adrift")]
        public async Task<IActionResult> Update_Adrift(AdriftRequest request)
        {
            var result = await _advertTrafficMasterService.UpdateAdrift(request, _getJwtTokenInfo.UserId);
            return Ok(result);
        }

        #endregion

        #region 全站廣告彈窗 (Type=1)

        /// <summary>
        ///     流量主後台-我的廣告-取得全站廣告彈窗
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("allsitepopup")]
        public async Task<IActionResult> GetAllSitePopUp()
        {
            var result = await _advertTrafficMasterService.GetAllSitePopUp(_getJwtTokenInfo.UserId);
            return Ok(result);
        }

        /// <summary>
        ///     流量主後台-我的廣告-更新全站廣告彈窗
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("allsitepopup")]
        public async Task<IActionResult> UpdateAllSitePopUp(AllSitePopUpdateRequest request)
        {
            var result = await _advertTrafficMasterService.UpdateAllSitePopUp(request, _getJwtTokenInfo.UserId);
            return Ok(result);
        }

        #endregion

        #region 片頭彈窗廣告 (Type=2)

        /// <summary>
        ///     流量主後台-我的廣告-取得片頭彈窗廣告
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("videopopup")]
        public async Task<IActionResult> GetVideoPopUp()
        {
            var result = await _advertTrafficMasterService.GetVideoPopUp(_getJwtTokenInfo.UserId);
            return Ok(result);
        }

        /// <summary>
        ///     流量主後台-我的廣告-更新片頭彈窗廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("videopopup")]
        public async Task<IActionResult> UpdateVideoPopUp(VideoPopUpUpdateRequest request)
        {
            var result = await _advertTrafficMasterService.UpdateVideoPopUp(request, _getJwtTokenInfo.UserId);
            return Ok(result);
        }

        #endregion

        #region 上方橫幅廣告 (Type=3)

        /// <summary> 流量主後台-我的廣告-取得上方橫幅廣告列表 </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("topbanner")]
        public async Task<IActionResult> GetTopBannerList(int index = 1, int size = 30)
        {
            var result = await _advertTrafficMasterService.Get_Top_Banner_Record(index, size, _getJwtTokenInfo.UserId);
            return Ok(result);
        }


        /// <summary>
        ///     流量主後台-我的廣告-新增上方橫幅廣告
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("topbanner")]
        public async Task<IActionResult> AddTopBanner(AddTopBannerRequest request)
        {
            var result = await _advertTrafficMasterService.Add_Top_Banner_Record(request, _getJwtTokenInfo.UserId);
            return Ok(result);
        }


        /// <summary>
        ///     流量主後台-我的廣告-更新上方橫幅廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("topbanner")]
        public async Task<IActionResult> UpdateTopBanner(UpdateTopBannerRequest request)
        {
            var result = await _advertTrafficMasterService.Update_Top_Banner_Record(request, _getJwtTokenInfo.UserId);
            return Ok(result);
        }


        /// <summary>
        ///     流量主後台-我的廣告-刪除上方橫幅廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("topbanner/{id}")]
        public async Task<IActionResult> DelTopBanner(int id)
        {
            var result = await _advertTrafficMasterService.del_Top_Banner_Record(id, _getJwtTokenInfo.UserId);
            return Ok(result);
        }

        #endregion

        #region 影片Banner (Type=4)

        /// <summary>
        ///     流量主後台-我的廣告-取得影片Banner
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("videobanner")]
        public async Task<IActionResult> GetVideoBannerList(int index = 1, int size = 30)
        {
            var result = await _advertTrafficMasterService.Get_Video_Banner_Record(index, size, _getJwtTokenInfo.UserId);
            return Ok(result);
        }


        /// <summary>
        ///     流量主後台-我的廣告-新增影片Banner
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("videobanner")]
        public async Task<IActionResult> AddVideoBanner(AddVideoBannerRequest request)
        {
            var result = await _advertTrafficMasterService.Add_Video_Banner_Record(request, _getJwtTokenInfo.UserId);
            return Ok(result);
        }


        /// <summary>
        ///     流量主後台-我的廣告-更新影片Banner
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("videobanner")]
        public async Task<IActionResult> UpdateVideoBanner(UpdateVideoBannerRequest request)
        {
            var result = await _advertTrafficMasterService.Update_Video_Banner_Record(request, _getJwtTokenInfo.UserId);
            return Ok(result);
        }


        /// <summary>
        ///     流量主後台-我的廣告-刪除影片Banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("videobanner/{id}")]
        public async Task<IActionResult> DelVideoBanner(int id)
        {
            var result = await _advertTrafficMasterService.del_Video_Banner_Record(id, _getJwtTokenInfo.UserId);
            return Ok(result);
        }

        #endregion

        #region 全民贊助廣告

        /// <summary> 流量主後台-取得全民贊助廣告列表 </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("sponsor")]
        public async Task<IActionResult> Get_SponsorList(int index = 1, int size = 30)
        {
            //如果設定檔是關閉的,則讀取union_print
            var check_is_Enable = await _advertConfigService.sponsor_isEnable();
            if (check_is_Enable.data)
            {
                var s_result = await _sponsorAdvertTraffic.Get_Print_Record(_getJwtTokenInfo.UserId, index, size);
                return await Task.FromResult<IActionResult>(Ok(s_result));
            }

            var n_result = await _sponsorAdvertService.Get_Print_Record(index, size);
            return Ok(n_result);
        }

        /// <summary>
        ///     流量主後台-新增全民贊助廣告
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("sponsor")]
        public async Task<IActionResult> AddSponsor(SponsorTrafficMasterRequest trafficMasterRequest)
        {
            var check_is_Enable = await _advertConfigService.sponsor_isEnable();
            if (!check_is_Enable.data) return Unauthorized();

            var result = await _sponsorAdvertTraffic.Add_Print_Record(_getJwtTokenInfo.UserId, trafficMasterRequest);
            return Ok(result);
        }

        /// <summary>
        ///     流量主後台-更新全民贊助廣告
        /// </summary>
        /// <param name="trafficMasterRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("sponsor")]
        public async Task<IActionResult> UpdateSponsor(UpdateSponsorTrafficMasterRequest trafficMasterRequest)
        {
            var check_is_Enable = await _advertConfigService.sponsor_isEnable();
            if (!check_is_Enable.data) return Unauthorized();
            var result = await _sponsorAdvertTraffic.Update_Print_Record(_getJwtTokenInfo.UserId, trafficMasterRequest);
            return Ok(result);
        }

        /// <summary>
        ///     流量主後台-刪除全民贊助廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("sponsor/{id}")]
        public async Task<IActionResult> Del_sponsor(int id)
        {
            var check_is_Enable = await _advertConfigService.sponsor_isEnable();
            if (!check_is_Enable.data) return Unauthorized();
            var result = await _sponsorAdvertTraffic.Del_Print_Record(id);
            return Ok(result);
        }

        #endregion
    }
}