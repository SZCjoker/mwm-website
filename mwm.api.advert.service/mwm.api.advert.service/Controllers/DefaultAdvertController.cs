using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MWM.API.Advert.Service.Application.AdvertConfig;
using MWM.API.Advert.Service.Application.DefaultAdvert;
using MWM.API.Advert.Service.Application.DefaultAdvert.Contract;
using MWM.API.Advert.Service.Application.SponsorAdvert;
using MWM.API.Advert.Service.Application.SponsorAdvert.Contract;


namespace MWM.API.Advert.Service.Controllers
{
    /// <summary>官網後台-操作預設廣告API</summary>
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultAdvertController : ControllerBase
    {
        private readonly IDefaultAdvertService _defaultAdvertService;
        private readonly ISponsorAdvertService _sponsorAdvertService;
        private readonly IAdvertConfigService _advertConfigService;

        ///  <summary>
        /// 後台人員操作預設廣告
        ///  </summary>
        ///  <param name="defaultAdvertService"></param>
        ///  <param name="sponsorAdvertService"></param>
        ///  <param name="advertConfigService"></param>
        public DefaultAdvertController(IDefaultAdvertService defaultAdvertService, ISponsorAdvertService sponsorAdvertService, IAdvertConfigService advertConfigService)
        {
            _defaultAdvertService = defaultAdvertService;
            _sponsorAdvertService = sponsorAdvertService;
            _advertConfigService = advertConfigService;
        }

        #region 漂浮廣告(Type=0) 

        /// <summary> 官網後台-取得漂浮廣告列表</summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Adrift")]
        public async Task<IActionResult> Get_Adrift()
        {
            var result = await _defaultAdvertService.GetAdriftList();
            return Ok(result);
        }

        /// <summary> 官網後台-更新漂浮廣吿 </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("adrift")]
        public async Task<IActionResult> Update_Adrift(AdriftRequest request)
        {
            var result = await _defaultAdvertService.UpdateAdrift(request);
            return Ok(result);
        }

        #endregion
        
        #region 全站廣告彈窗 (Type=1)

        /// <summary>
        ///     官網後台-取得全站廣告彈窗
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("allsitepopup")]
        public async Task<IActionResult> GetAllSitePopUp()
        {
            var result = await _defaultAdvertService.GetAllSitePopUp();
            return Ok(result);
        }

        /// <summary>
        ///     官網後台-更新全站廣告彈窗
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("allsitepopup")]
        public async Task<IActionResult> UpdateAllSitePopUp(AllSitePopUpdateRequest request)
        {
            var result = await _defaultAdvertService.UpdateAllSitePopUp(request);
            return Ok(result);
        }

        #endregion

        #region 片頭彈窗廣告 (Type=2)

        /// <summary>
        ///    官網後台- 取得片頭彈窗廣告
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("videopopup")]
        public async Task<IActionResult> GetVideoPopUp()
        {
            var result = await _defaultAdvertService.GetVideoPopUp();
            return Ok(result);
        }

        /// <summary>
        ///    官網後台- 更新片頭彈窗廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("videopopup")]
        public async Task<IActionResult> UpdateVideoPopUp(VideoPopUpUpdateRequest request)
        {
            var result = await _defaultAdvertService.UpdateVideoPopUp(request);
            return Ok(result);
        }

        #endregion

        #region 上方橫幅廣告 (Type=3)

        /// <summary> 官網後台-取得上方橫幅廣告列表 </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("topbanner")]
        public async Task<IActionResult> GetTopBannerList(int index = 1, int size = 30)
        {
            var result = await _defaultAdvertService.Get_Top_Banner_Record(index, size);
            return Ok(result);
        }


        /// <summary>
        ///     官網後台-新增上方橫幅廣告
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("topbanner")]
        public async Task<IActionResult> AddTopBanner(AddTopBannerRequest request)
        {
            var result = await _defaultAdvertService.Add_Top_Banner_Record(request);
            return Ok(result);
        }


        /// <summary>
        ///     官網後台-更新上方橫幅廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("topbanner")]
        public async Task<IActionResult> UpdateTopBanner(UpdateTopBannerRequest request)
        {
            var result = await _defaultAdvertService.Update_Top_Banner_Record(request);
            return Ok(result);
        }


        /// <summary>
        ///     官網後台-刪除上方橫幅廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("topbanner/{id}")]
        public async Task<IActionResult> DelTopBanner(int id)
        {
            var result = await _defaultAdvertService.del_Top_Banner_Record(id);
            return Ok(result);
        }

        #endregion

        #region 影片Banner (Type=4)

        /// <summary>
        /// 官網後台-取得影片Banner
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("videobanner")]
        public async Task<IActionResult> GetVideoBannerList(int index = 1, int size = 30)
        {
            var result = await _defaultAdvertService.Get_Video_Banner_Record(index, size);
            return Ok(result);
        }


        /// <summary>
        ///官網後台-新增影片Banner
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("videobanner")]
        public async Task<IActionResult> AddVideoBanner(AddVideoBannerRequest request)
        {
            var result = await _defaultAdvertService.Add_Video_Banner_Record(request);
            return Ok(result);
        }


        /// <summary>
        ///官網後台-更新影片Banner
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("videobanner")]
        public async Task<IActionResult> UpdateVideoBanner(UpdateVideoBannerRequest request)
        {
            var result = await _defaultAdvertService.Update_Video_Banner_Record(request);
            return Ok(result);
        }


        /// <summary>
        ///官網後台-刪除影片Banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("videobanner/{id}")]
        public async Task<IActionResult> DelVideoBanner(int id)
        {
            var result = await _defaultAdvertService.del_Video_Banner_Record(id);
            return Ok(result);
        }

        #endregion
        
        #region 全民贊助廣告
        /// <summary>
        /// 官網後台-新增全民贊助廣告
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("sponsor")]
        public async Task<IActionResult> AddPrint(SponsorAdvertRequest request)
        {
            var result = await _sponsorAdvertService.Add_Print_Record(request);
            return Ok(result);
        }
        
        /// <summary>
        /// 官網後台-更新全民贊助廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("sponsor")]
        public async Task<IActionResult> UpdatePrint(UpdateSponsorAdvertRequest request)
        {
            var result = await _sponsorAdvertService.Update_Print_Record(request);
            return Ok(result);
        }
        /// <summary>
        /// 官網後台-刪除全民贊助廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("sponsor/{id}")]
        public async Task<IActionResult> DelPrint(int id)
        {
            var result = await _sponsorAdvertService.Del_Print_Record(id);
            return Ok(result);
        }
        
        
        /// <summary> 官網後台-取得全民贊助廣告列表</summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("sponsor")]
        public async Task<IActionResult> GetPrintAll(int index = 1, int size = 30)
        {
            var result = await _sponsorAdvertService.Get_Print_Record(index, size);
            return Ok(result);
        }
        #endregion

        #region 全民贊助廣告相關設定
        /// <summary>
        /// 官網後台-開啟或關閉全民贊助廣告
        /// </summary>
        /// <param name="enable_status"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("sponsorsetting")]
        public async Task<IActionResult> UpdateSponsorEnable(bool enable_status )
        {
            var result = await _advertConfigService.Update(enable_status);
            return Ok(result);
        }
        
        /// <summary>
        /// 官網後台-全民贊助廣告狀態
        /// </summary>
        /// <param name="enable_status"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("sponsorsetting/status")]
        public async Task<IActionResult> SponsorStatus(bool enable_status )
        {
            var result = await _advertConfigService.sponsor_isEnable();
            return Ok(result);
        }

        #endregion
        
        
    }
}