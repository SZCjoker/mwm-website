using System;
using System.Linq;
using System.Threading.Tasks;
using MWM.API.Advert.Service.Application.SponsorAdvert.Contract;
using MWM.API.Advert.Service.Domain.SponsorAdvert;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Advert.Service.Application.SponsorAdvert
{
    /// <summary>
    /// 後台操作-廣告聯盟  
    /// </summary>
    public class SponsorAdvertService:ISponsorAdvertService
    {
        private readonly ISponsorAdvertRepository _sponsorAdvertRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sponsorAdvertRepository"></param>
        public SponsorAdvertService(ISponsorAdvertRepository sponsorAdvertRepository)
        {
            _sponsorAdvertRepository = sponsorAdvertRepository;
        }
        /// <summary>
        /// 新增平面廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Add_Print_Record(SponsorAdvertRequest request)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            
            var record = new SponsorAdverEntity
            {
               
                image_link= request.image_link,
                hyper_link=request.hyper_link,
                device_type = request.device_type,
                position_type = request.position_type,
                desc=request.desc,
                ctime = create_time,
                utime = create_time
              
            };
             var cnt =await   _sponsorAdvertRepository.Add_Record(record);
            return StateCodeHandler.ForCount(cnt);
        }
        /// <summary>
        /// 更新平面廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Update_Print_Record(UpdateSponsorAdvertRequest request)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var record = new SponsorAdverEntity()
            {
                id  =request.id,
                image_link= request.image_link,
                hyper_link=request.hyper_link,
                device_type = request.device_type,
                position_type = request.position_type,
                desc=request.desc,
                utime = create_time
              
            };
            var cnt = await _sponsorAdvertRepository.Update_Record(record);
            return StateCodeHandler.ForCount(cnt);
        }
        /// <summary>
        /// 刪除平面廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Del_Print_Record(int id)
        {
            var cnt = await _sponsorAdvertRepository.Del_Record(id);

            return StateCodeHandler.ForCount(cnt);
        }
        /// <summary>
        /// 取得平面廣告列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<IPagingResult<SponsorAdvertResponse>> Get_Print_Record(int index, int size)
        {
            var requestPaging = new Paging(index, size);

            var (record, count) = await _sponsorAdvertRepository.GetRecords(string.Empty,  requestPaging.Offset, size);
            return StateCodeHandler.PagingRecord(record.OrderBy(o => o.id).Select(s => new SponsorAdvertResponse
            {
                id = s.id,
                device_type =s.device_type ,
                image_link = s.image_link,
                hyper_link = s.hyper_link,
                utime = s.utime,
                position_type = s.position_type,
                desc = s.desc
            }), new Paging(requestPaging.PageIndex, requestPaging.PageSize, count));
            
            
            
            
        }
    }
}