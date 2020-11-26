using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MWM.API.Advert.Service.Application.VideoAdvert.video;
using MWM.API.Advert.Service.Application.VideoAdvert.video.Contract;

namespace MWM.API.Advert.Service.Controllers
{
    /// <summary>
    /// 官網後台-人員操作影片廣告區域API
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class VideoAdvertController: ControllerBase
    {
        private readonly IVideoAdvertVideoService _videoAdvertVideoService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="videoAdvertVideoService"></param>
        public VideoAdvertController(IVideoAdvertVideoService videoAdvertVideoService)
        {
            _videoAdvertVideoService = videoAdvertVideoService;
        }

        #region 跑馬燈廣告
        /// <summary>
        /// 官網後台-新增跑馬燈
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("marquee")]
        public async Task<IActionResult> AddMarquee(MarqueeRequest request)
        {
            var result = await _videoAdvertVideoService.Add_Marquee_Record(request);
            return await Task.FromResult<IActionResult>(Ok(result));
        }

        /// <summary>
        /// 官網後台-更新跑馬燈
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("marquee")]
        public async Task<IActionResult> UpdateMarquee(UpdateMarqueeRequest request)
        {
            var result = await _videoAdvertVideoService.Update_Marquee_Record(request);
            return await Task.FromResult<IActionResult>(Ok(result));
        }

        
        /// <summary>
        /// 官網後台-刪除跑馬燈
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("marquee/{id}")]
        public async Task<IActionResult> DelMarquee(int id)
        {
            var result = await _videoAdvertVideoService.Del_Marquee_Record(id);
            return await Task.FromResult<IActionResult>(Ok(result));
        }

        
        #endregion

        #region 影片廣告
        /// <summary>
        /// 官網後台-新增影片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("video")]
        public async Task<IActionResult> AddVideo(VideoRequest request)
        {
            var result = await _videoAdvertVideoService.Add_Video_Record(request);
            return await Task.FromResult<IActionResult>(Ok(result));
        }

        /// <summary>
        /// 官網後台-更新影片
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("video")]
        public async Task<IActionResult> UpdateVideo(UpdateVideoRequest request)
        {
            var result = await _videoAdvertVideoService.Update_Video_Record(request);
            return await Task.FromResult<IActionResult>(Ok(result));
        }

        
        /// <summary>
        /// 官網後台-刪除影片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("video/{id}")]
        public async Task<IActionResult> DelVideo(int id)
        {
            var result = await _videoAdvertVideoService.Del_Video_Record(id);
            return await Task.FromResult<IActionResult>(Ok(result));
        }

        /// <summary> 官網後台-取得影片廣告列表 </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("videoall")]
        public async Task<IActionResult> GetVideoAll(int index = 1, int size = 30)
        {
            var result = await _videoAdvertVideoService.Get_VideoAll_Record(index, size);
            return await Task.FromResult<IActionResult>(Ok(result));
        }
        #endregion
        

   
    }
}