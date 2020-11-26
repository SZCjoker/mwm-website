using System.Collections.Generic;
using System.Threading.Tasks;
using MWM.API.Advert.Service.Domain.VideoAdvert.video;

namespace MWM.API.Advert.Service.Domain.VideoAdvert
{
    /// <summary>
    /// 官網後台-影片區域操作
    /// </summary>
    public interface IVideoAdvertRepository 
    {
        /// <summary>
        /// 取列表
        /// </summary>
        /// <param name="query_str"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<(IEnumerable<VideoAdvertEntity> record, long count)> GetRecords( string query_str, int index, int size);
        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> Del_Record(int id);
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> Update_Record(VideoAdvertEntity entity);
        
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        Task<int> Add_Record(VideoAdvertEntity entity);
    }
}