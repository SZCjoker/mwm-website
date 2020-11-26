using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MWM.API.Advert.Service.Application.AdvertTrafficMaster.Contract;
using MWM.API.Advert.Service.Domain.AdvertTrafficMaster;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Advert.Service.Application.AdvertTrafficMaster
{
    /// <summary>
    /// 流量主後台-我的廣告
    /// </summary>
    public class AdvertTrafficMasterService:IAdvertTrafficMasterService
    {
        private readonly IAdvertTrafficMasterRepository _advertTrafficMasterRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="advertTrafficMasterRepository"></param>
        public AdvertTrafficMasterService(IAdvertTrafficMasterRepository advertTrafficMasterRepository)
        {
            _advertTrafficMasterRepository = advertTrafficMasterRepository;
        }

        #region 上方橫幅廣告
        /// <summary>
        /// 流量主後台-我的廣告-新增上方橫幅廣告
        /// </summary>
        /// <param name="request"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Add_Top_Banner_Record(AddTopBannerRequest request, int account_id)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new AdvertTrafficMasterEntity
            {
                account_id = account_id,
                default_ads_type = 3,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc = string.IsNullOrEmpty(request.desc)?"":request.desc,
                banner_sort = request.banner_sort,
                ctime = create_time,
                utime = create_time
            };
            var cnt = await _advertTrafficMasterRepository.Add_Record(record);
            return StateCodeHandler.ForCount(cnt);
            
        }
        /// <summary>
        /// 流量主後台-我的廣告-更新上方橫幅廣告
        /// </summary>
        /// <param name="request"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Update_Top_Banner_Record(UpdateTopBannerRequest request, int account_id)
        {
            var u_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new AdvertTrafficMasterEntity
            {
                id = request.id,
                account_id = account_id,
                default_ads_type = 3,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc =  string.IsNullOrEmpty(request.desc)?"":request.desc,
                banner_sort = request.banner_sort,
                state = 1,
                utime = u_time
            };

            var cnt = await _advertTrafficMasterRepository.Update_Record(record, account_id);

            return StateCodeHandler.ForCount(cnt);
        }
        /// <summary>
        /// 流量主後台-我的廣告-刪除上方橫幅廣告
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> del_Top_Banner_Record(int id, int account_id)
        {
            var cnt = await _advertTrafficMasterRepository.Del_Record(id, 3);

            return StateCodeHandler.ForCount(cnt);
        }
        /// <summary>
        ///  流量主後台-我的廣告-取得上方橫幅廣告列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<IPagingResult<TopBannerResponse>> Get_Top_Banner_Record(int index, int size, int account_id)
        {
            var requestPaging = new Paging(index, size);

            var (record, count) = await _advertTrafficMasterRepository.GetRecords(account_id, 3, requestPaging.Offset, size);
            return StateCodeHandler.PagingRecord(record.OrderBy(o => o.banner_sort).Select(s => new TopBannerResponse
            {
                id = s.id,
                hyper_link = s.hyper_link,
                image_link = s.image_link,
                desc = s.desc,
                banner_sort = s.banner_sort,
                utime = s.utime
            }), new Paging(requestPaging.PageIndex, requestPaging.PageSize, count));
        }
        

        #endregion

        #region 漂浮廣告
        /// <summary>
        /// 流量主後台-我的廣告-取得漂浮廣告列表
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<BasicResponse<IEnumerable<AdriftResponse>>> GetAdriftList(int account_id)
        {
            var (record, count) = await _advertTrafficMasterRepository.GetRecords(account_id, 0, 0, 4);


            if (record == null)
                return StateCodeHandler.ForNull<IEnumerable<AdriftResponse>>(null);

            if (count == 0)
                return StateCodeHandler.ForNull<IEnumerable<AdriftResponse>>(null);


            return StateCodeHandler.ForNull(record.OrderByDescending(o => o.ctime).Select(s => new AdriftResponse
            {
                id = s.id,
                position = s.position,
                hyper_link = s.hyper_link,
                image_link = s.image_link,
                desc = s.desc,
                status = s.state,
                utime = s.utime
            }));
        }
        /// <summary>
        /// 流量主後台-我的廣告-更新漂浮廣告
        /// </summary>
        /// <param name="request"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> UpdateAdrift(AdriftRequest request, int account_id)
        {
            var u_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new AdvertTrafficMasterEntity
            {
                id = request.id,
                account_id = account_id,
                default_ads_type = 0,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc = request.desc,
                state = request.status,
                utime = u_time
            };

            var cnt = await _advertTrafficMasterRepository.Update_Record(record, account_id);

            return StateCodeHandler.ForCount(cnt);
        }
        

        #endregion

        #region 全站彈跳視窗
        /// <summary>
        /// 流量主後台-我的廣告-取得全站彈跳視窗
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<BasicResponse<AllSitePopUpResponse>> GetAllSitePopUp(int account_id)
        {
            var (record, count) = await _advertTrafficMasterRepository.GetRecords(account_id, 1, 0, 1);
            if (record == null)
                return StateCodeHandler.ForNull<AllSitePopUpResponse>(null);

            if (count == 0)
                return StateCodeHandler.ForNull<AllSitePopUpResponse>(null);


            return StateCodeHandler.ForNull(record.Select(s => new AllSitePopUpResponse
            {
                id = s.id,
                hyper_link = s.hyper_link,
                image_link = s.image_link,
                desc = s.desc,
                status = s.state,
                utime = s.utime
            }).FirstOrDefault());
        }
        /// <summary>
        /// 流量主後台-我的廣告- 更新全站彈跳視窗
        /// </summary>
        /// <param name="request"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> UpdateAllSitePopUp(AllSitePopUpdateRequest request, int account_id)
        {
            var u_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
       
            var record = new AdvertTrafficMasterEntity
            {
                id = request.id,
                account_id = account_id,
                default_ads_type = 1,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc = request.desc,
                state = request.status,
                utime = u_time
            };
       
            var cnt = await _advertTrafficMasterRepository.Update_Record(record, account_id);
       
            return StateCodeHandler.ForCount(cnt);
        }
        

        #endregion

        #region 片頭彈窗廣告

        /// <summary>
        /// 流量主後台-我的廣告-取得片頭彈窗廣告
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<BasicResponse<VideoPopUpResponse>> GetVideoPopUp(int account_id)
        {
            var (record, count) = await _advertTrafficMasterRepository.GetRecords(account_id, 2, 0, 1);
            if (record == null)
                return StateCodeHandler.ForNull<VideoPopUpResponse>(null);

            if (count == 0)
                return StateCodeHandler.ForNull<VideoPopUpResponse>(null);


            return StateCodeHandler.ForNull(record.Select(s => new VideoPopUpResponse
            {
                id = s.id,
                hyper_link = s.hyper_link,
                image_link = s.image_link,
                desc = s.desc,
                status = s.state,
                utime = s.utime
            }).FirstOrDefault());
        }
        /// <summary>
        /// 流量主後台-我的廣告-更新片頭彈窗廣告
        /// </summary>
        /// <param name="request"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> UpdateVideoPopUp(VideoPopUpUpdateRequest request, int account_id)
        {
            var u_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new AdvertTrafficMasterEntity
            {
                id = request.id,
                default_ads_type = 2,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc = request.desc,
                state = request.status,
                utime = u_time
            };

            var cnt = await _advertTrafficMasterRepository.Update_Record(record, account_id);

            return StateCodeHandler.ForCount(cnt);
        }
        

        #endregion

        #region 影片banner
        /// <summary>
        /// 流量主後台-我的廣告-新增影片banner
        /// </summary>
        /// <param name="request"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Add_Video_Banner_Record(AddVideoBannerRequest request, int account_id)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new AdvertTrafficMasterEntity
            {
                default_ads_type = 4,
                account_id = account_id,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc = string.IsNullOrEmpty(request.desc)?"":request.desc,
                ctime = create_time,
                utime = create_time
            };
            var cnt = await _advertTrafficMasterRepository.Add_Record(record);
            return StateCodeHandler.ForCount(cnt);
        }
        /// <summary>
        /// 流量主後台-我的廣告-更新影片banner
        /// </summary>
        /// <param name="request"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Update_Video_Banner_Record(UpdateVideoBannerRequest request, int account_id)
        {
            var u_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new AdvertTrafficMasterEntity
            {
                id = request.id,
                account_id = account_id,
                default_ads_type = 4,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc =  string.IsNullOrEmpty(request.desc)?"":request.desc,
                state = 1,
                utime = u_time
            };

            var cnt = await _advertTrafficMasterRepository.Update_Record(record, account_id);

            return StateCodeHandler.ForCount(cnt);
        }
        /// <summary>
        /// 流量主後台-我的廣告-刪除影片banner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> del_Video_Banner_Record(int id, int account_id)
        {
            var cnt = await _advertTrafficMasterRepository.Del_Record(id, 4);

            return StateCodeHandler.ForCount(cnt);
        }
        /// <summary>
        /// 流量主後台-我的廣告-取得影片banner列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        public async Task<IPagingResult<AddVideoBannerResponse>> Get_Video_Banner_Record(int index, int size, int account_id)
        {
            var requestPaging = new Paging(index, size);

            var (record, count) = await _advertTrafficMasterRepository.GetRecords(account_id, 4, requestPaging.Offset, size);
            return StateCodeHandler.PagingRecord(record.OrderBy(o => o.id).Select(s => new AddVideoBannerResponse
            {
                id = s.id,
                hyper_link = s.hyper_link,
                image_link = s.image_link,
                desc = s.desc,
                utime = s.utime
            }), new Paging(requestPaging.PageIndex, requestPaging.PageSize, count));
        }
        

        #endregion

    }
}