using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MWM.API.Advert.Service.Application.DefaultAdvert.Contract;
using MWM.API.Advert.Service.Domain.DefaultAdvert;
using Phoenixnet.Extensions.Handler;
using Phoenixnet.Extensions.Object;

namespace MWM.API.Advert.Service.Application.DefaultAdvert
{
    /// <summary>
    /// 預設廣告實作
    /// </summary>
    public class DefaultAdvertService : IDefaultAdvertService
    {
        private readonly IDefaultAdvertRepository _defaultAdvertRepository;

        /// <summary>
        /// </summary>
        /// <param name="defaultAdvertRepository"></param>
        public DefaultAdvertService(IDefaultAdvertRepository defaultAdvertRepository)
        {
            _defaultAdvertRepository = defaultAdvertRepository;
        }


        #region 全站彈跳視窗

        /// <summary>
        ///     取得全站彈跳視窗
        /// </summary>
        /// <returns></returns>
        public async Task<BasicResponse<AllSitePopUpResponse>> GetAllSitePopUp()
        {
            var (record, count) = await _defaultAdvertRepository.GetRecords(string.Empty, 1, 0, 1);
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
        ///     更新全站彈跳視窗
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> UpdateAllSitePopUp(AllSitePopUpdateRequest request)
        {
            var u_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new DefaultAdvertEntity
            {
                id = request.id,
                default_ads_type = 1,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc = request.desc,
                state = request.status,
                utime = u_time
            };

            var cnt = await _defaultAdvertRepository.Update_Record(record);

            return StateCodeHandler.ForCount(cnt);
        }

        #endregion

        #region 片頭彈窗廣告

        /// <summary>
        ///     取得片頭彈窗廣告
        /// </summary>
        /// <returns></returns>
        public async Task<BasicResponse<VideoPopUpResponse>> GetVideoPopUp()
        {
            var (record, count) = await _defaultAdvertRepository.GetRecords(string.Empty, 2, 0, 1);
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
        ///     更新片頭彈窗廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> UpdateVideoPopUp(VideoPopUpUpdateRequest request)
        {
            var u_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new DefaultAdvertEntity
            {
                id = request.id,
                default_ads_type = 2,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc = request.desc,
                state = request.status,
                utime = u_time
            };

            var cnt = await _defaultAdvertRepository.Update_Record(record);

            return StateCodeHandler.ForCount(cnt);
        }

        #endregion

        #region 上方橫幅廣告

        /// <summary>
        ///     新增上方橫幅廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Add_Top_Banner_Record(AddTopBannerRequest request)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new DefaultAdvertEntity
            {
                default_ads_type = 3,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc = string.IsNullOrEmpty(request.desc)?"":request.desc,
                banner_sort = request.banner_sort,
                ctime = create_time,
                utime = create_time
            };
            var cnt = await _defaultAdvertRepository.Add_Record(record);
            return StateCodeHandler.ForCount(cnt);
        }

        /// <summary>
        ///     更新上方橫幅廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Update_Top_Banner_Record(UpdateTopBannerRequest request)
        {
            var u_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new DefaultAdvertEntity
            {
                id = request.id,
                default_ads_type = 3,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc = string.IsNullOrEmpty(request.desc)?"":request.desc,
                banner_sort = request.banner_sort,
                state = 1,
                utime = u_time
            };

            var cnt = await _defaultAdvertRepository.Update_Record(record);

            return StateCodeHandler.ForCount(cnt);
        }

        /// <summary>
        ///     刪除上方橫幅廣告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> del_Top_Banner_Record(int id)
        {
            var cnt = await _defaultAdvertRepository.Del_Record(id, 3);

            return StateCodeHandler.ForCount(cnt);
        }

        /// <summary>
        ///     取得上方橫幅廣告列表
        /// </summary>
        /// <returns></returns>
        public async Task<IPagingResult<TopBannerResponse>> Get_Top_Banner_Record(int index, int size)
        {
            var requestPaging = new Paging(index, size);

            var (record, count) = await _defaultAdvertRepository.GetRecords(string.Empty, 3, requestPaging.Offset, size);
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
        ///     取得漂浮廣告列表
        /// </summary>
        /// <returns></returns>
        public async Task<BasicResponse<IEnumerable<AdriftResponse>>> GetAdriftList()
        {
            var (record, count) = await _defaultAdvertRepository.GetRecords(string.Empty, 0, 0, 4);


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
        ///     更新漂浮廣告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> UpdateAdrift(AdriftRequest request)
        {
            var u_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new DefaultAdvertEntity
            {
                id = request.id,
                default_ads_type = 0,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc = request.desc,
                state = request.status,
                utime = u_time
            };

            var cnt = await _defaultAdvertRepository.Update_Record(record);

            return StateCodeHandler.ForCount(cnt);
        }

        #endregion

        #region 影片banner

        /// <summary>
        ///     新增影片banner
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Add_Video_Banner_Record(AddVideoBannerRequest request)
        {
            var create_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new DefaultAdvertEntity
            {
                default_ads_type = 4,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc = string.IsNullOrEmpty(request.desc)?"":request.desc,
                ctime = create_time,
                utime = create_time
            };
            var cnt = await _defaultAdvertRepository.Add_Record(record);
            return StateCodeHandler.ForCount(cnt);
        }

        /// <summary>
        ///     更新影片banner
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BasicResponse> Update_Video_Banner_Record(UpdateVideoBannerRequest request)
        {
            var u_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var record = new DefaultAdvertEntity
            {
                id = request.id,
                default_ads_type = 4,
                hyper_link = request.hyper_link,
                image_link = request.image_link,
                desc = string.IsNullOrEmpty(request.desc)?"":request.desc,
                state = 1,
                utime = u_time
            };

            var cnt = await _defaultAdvertRepository.Update_Record(record);

            return StateCodeHandler.ForCount(cnt);
        }

        /// <summary>
        ///     刪除影片banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BasicResponse> del_Video_Banner_Record(int id)
        {
            var cnt = await _defaultAdvertRepository.Del_Record(id, 4);

            return StateCodeHandler.ForCount(cnt);
        }

        /// <summary>
        ///     取得影片banner列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<IPagingResult<AddVideoBannerResponse>> Get_Video_Banner_Record(int index, int size)
        {
            var requestPaging = new Paging(index, size);

            var (record, count) = await _defaultAdvertRepository.GetRecords(string.Empty, 4, requestPaging.Offset, size);
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