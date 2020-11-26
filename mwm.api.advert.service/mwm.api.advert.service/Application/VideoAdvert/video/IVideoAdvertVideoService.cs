using System.Threading.Tasks;
using MWM.API.Advert.Service.Application.VideoAdvert.video.Contract;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Advert.Service.Application.VideoAdvert.video
{
    /// <summary>
    ///  官網後台-影片廣告區域
    /// </summary>
    public interface IVideoAdvertVideoService
    {
        /// <summary>
        ///新增跑馬燈廣告
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> Add_Marquee_Record(MarqueeRequest request);

        /// <summary>
        /// 更新跑馬燈廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BasicResponse> Update_Marquee_Record(UpdateMarqueeRequest request);
        
        /// <summary>
        /// 刪除跑馬燈廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BasicResponse> Del_Marquee_Record(int id);
        
        /// <summary>
        ///新增影片廣告
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> Add_Video_Record(VideoRequest request);
        
        /// <summary>
        ///更新影片廣告
        /// </summary>
        /// <returns></returns>
        Task<BasicResponse> Update_Video_Record(UpdateVideoRequest request);
        
        /// <summary>
        /// 刪除影片廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BasicResponse> Del_Video_Record(int id);
        
        /// <summary>
        /// 取得影片廣告列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<IPagingResult<VideoAllResponse>> Get_VideoAll_Record(int index, int size);
    }
}