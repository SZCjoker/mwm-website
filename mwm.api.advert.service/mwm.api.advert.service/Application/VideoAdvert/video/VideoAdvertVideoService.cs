using System;
using System.Linq;
using System.Threading.Tasks;
using MWM.API.Advert.Service.Application.VideoAdvert.video.Contract;
using MWM.API.Advert.Service.Domain.VideoAdvert;
using MWM.API.Advert.Service.Domain.VideoAdvert.video;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Advert.Service.Application.VideoAdvert.video
{
    /// <summary>
    /// 官網後台-影片廣告區域
    /// </summary>
    public class VideoAdvertVideoService:IVideoAdvertVideoService
    {
        private readonly IVideoAdvertRepository _videoAdvertRepository;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="videoAdvertRepository"></param>
        public VideoAdvertVideoService(IVideoAdvertRepository videoAdvertRepository)
        {
            _videoAdvertRepository = videoAdvertRepository;
        }

        #region 跑馬燈廣告
            
        /// <summary>
        /// 新增跑馬燈廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Add_Marquee_Record(MarqueeRequest request)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new VideoAdvertEntity
            {
              title = request.title,
              hyper_link=request.hyper_link,
              color=request.color,
              position = request.position,
              advert_text=request.advert_text,
              loop_type=request.loop_type,
              default_time=request.default_time,
              start_time=request.start_time,
              end_time = request.end_time,
              status=request.status,
              video_advert_type=0,
              desc=request.desc,
              ctime = create_time,
              utime = create_time
              
            };
            var cnt = await _videoAdvertRepository.Add_Record(record);
            return StateCodeHandler.ForCount(cnt);
            
            
        }

        /// <summary>
        /// 更新跑馬燈廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Update_Marquee_Record(UpdateMarqueeRequest request)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var record = new VideoAdvertEntity
            {
                id =request.id,
                title = request.title,
                hyper_link=request.hyper_link,
                color=request.color,
                position = request.position,
                advert_text=request.advert_text,
                loop_type=request.loop_type,
                default_time=request.default_time,
                start_time=request.start_time,
                end_time = request.end_time,
                status=request.status,
                video_advert_type=0,
                desc=request.desc,
                utime = create_time
              
            };
            var cnt = await _videoAdvertRepository.Update_Record(record);
            return StateCodeHandler.ForCount(cnt);
            
        }
        /// <summary>
        /// 刪除跑馬燈
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Del_Marquee_Record(int id)
        {
            var cnt = await _videoAdvertRepository.Del_Record(id);

            return StateCodeHandler.ForCount(cnt);
        }


        #endregion

        #region 影片廣告
        /// <summary>
        /// 新增影片廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Add_Video_Record(VideoRequest request)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new VideoAdvertEntity
            {
                title = request.title,
                hyper_link=request.hyper_link,
                skip_time= request.skip_time,
                video_link = request.video_link,
                default_time=request.default_time,
                start_time=request.start_time,
                end_time = request.end_time,
                status=request.status,
                video_advert_type=1,
                desc=request.desc,
                ctime = create_time,
                utime = create_time
              
            };
            var cnt = await _videoAdvertRepository.Add_Record(record);
            return StateCodeHandler.ForCount(cnt);
        }
        /// <summary>
        /// 更新影片廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Update_Video_Record(UpdateVideoRequest request)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var record = new VideoAdvertEntity
            {
                id =request.id,
                title = request.title,
                hyper_link=request.hyper_link,
                skip_time= request.skip_time,
                video_link = request.video_link,
                default_time=request.default_time,
                start_time=request.start_time,
                end_time = request.end_time,
                status=request.status,
                video_advert_type=1,
                desc=request.desc,
                utime = create_time
              
            };
            var cnt = await _videoAdvertRepository.Update_Record(record);
            return StateCodeHandler.ForCount(cnt);
        }
        /// <summary>
        /// 刪除影片廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Del_Video_Record(int id)
        {
            var cnt = await _videoAdvertRepository.Del_Record(id);

            return StateCodeHandler.ForCount(cnt);
        }


        /// <summary>
        /// 取得影片廣告列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<IPagingResult<VideoAllResponse>> Get_VideoAll_Record(int index, int size)
        {
            var requestPaging = new Paging(index, size);

            var (record, count) = await _videoAdvertRepository.GetRecords(string.Empty,  requestPaging.Offset, size);
            return StateCodeHandler.PagingRecord(record.OrderBy(o => o.id).Select(s => new VideoAllResponse
            {
                id = s.id,
                title = s.title,
                start_time = s.start_time,
                end_time = s.end_time,
                desc = s.desc,
                status = s.status,
                detail =s.video_advert_type==0?(object)  
                    new MarqueeRequest
                    {
                        title=s.title,
                        hyper_link = s.hyper_link,
                        color = s.color,
                        position = s.position,
                        advert_text = s.advert_text,
                        loop_type=s.loop_type,
                        default_time =s.default_time,
                        start_time=s.start_time,
                        end_time = s.end_time,
                        status = s.status,
                        desc = s.desc
                    } 
                    :
                    new VideoRequest
                    {
                        title = s.title,
                        hyper_link = s.hyper_link,
                        video_link = s.video_link,
                        skip_time = s.skip_time,
                        default_time = s.default_time,
                        start_time = s.start_time,
                        end_time = s.end_time,
                        status = s.status,
                        desc = s.desc
                    }
            }), new Paging(requestPaging.PageIndex, requestPaging.PageSize, count));
        }

        
        
        #endregion

    }
}