using System;
using System.Linq;
using System.Threading.Tasks;
using MWM.API.Advert.Service.Application.SponsorAdvertTrafficMaster.Contract;
using MWM.API.Advert.Service.Domain.SponsorAdvertTrafficMaster;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Advert.Service.Application.SponsorAdvertTrafficMaster
{
    /// <summary>
    /// 流量主-全民贊助廣告
    /// </summary>
    public class SponsorAdvertTrafficMasterService:ISponsorAdvertTrafficMasterService
    {
        private readonly ISponsorAdvertTrafficMasterRepository _sponsorAdvertTrafficMasterRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sponsorAdvertTrafficMasterRepository"></param>
        public SponsorAdvertTrafficMasterService(ISponsorAdvertTrafficMasterRepository sponsorAdvertTrafficMasterRepository)
        {
            _sponsorAdvertTrafficMasterRepository = sponsorAdvertTrafficMasterRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="trafficMasterRequest"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Add_Print_Record(int account_id,SponsorTrafficMasterRequest trafficMasterRequest)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            
            var record = new SponsorAdvertTrafficMasterEntity
            {
               
                image_link= trafficMasterRequest.image_link,
                account_id = account_id,
                hyper_link=trafficMasterRequest.hyper_link,
                device_type = trafficMasterRequest.device_type,
                position_type = trafficMasterRequest.position_type,
                desc=string.IsNullOrEmpty(trafficMasterRequest.desc)?"":trafficMasterRequest.desc,
                ctime = create_time,
                utime = create_time
              
            };
            var cnt =await   _sponsorAdvertTrafficMasterRepository.Add_Record(record);
            return StateCodeHandler.ForCount(cnt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="trafficMasterRequest"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Update_Print_Record(int account_id, UpdateSponsorTrafficMasterRequest trafficMasterRequest)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var record = new SponsorAdvertTrafficMasterEntity()
            {
                id  =trafficMasterRequest.id,
                account_id = account_id,
                image_link= trafficMasterRequest.image_link,
                hyper_link=trafficMasterRequest.hyper_link,
                device_type = trafficMasterRequest.device_type,
                position_type = trafficMasterRequest.position_type,
                desc=string.IsNullOrEmpty(trafficMasterRequest.desc)?"":trafficMasterRequest.desc,
                utime = create_time
              
            };
            var cnt = await _sponsorAdvertTrafficMasterRepository.Update_Record(record);
            return StateCodeHandler.ForCount(cnt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Del_Print_Record(int id)
        {
            var cnt = await _sponsorAdvertTrafficMasterRepository.Del_Record(id);
            return StateCodeHandler.ForCount(cnt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<IPagingResult<SponsorTrafficMasterResponse>> Get_Print_Record(int account_id, int index, int size)
        {
            var requestPaging = new Paging(index, size);

            var (record, count) = await _sponsorAdvertTrafficMasterRepository.GetRecords($"where account_id={account_id}",  requestPaging.Offset, size);
            return StateCodeHandler.PagingRecord(record.OrderBy(o => o.id).Select(s => new SponsorTrafficMasterResponse
            {
                id = s.id,
                account_id = s.account_id,
                image_link = s.image_link,
                hyper_link = s.hyper_link,
                utime = s.utime,
                device_type = s.device_type,
                position_type = s.position_type,
                desc = s.desc
            }), new Paging(requestPaging.PageIndex, requestPaging.PageSize, count));
        }
    }
}