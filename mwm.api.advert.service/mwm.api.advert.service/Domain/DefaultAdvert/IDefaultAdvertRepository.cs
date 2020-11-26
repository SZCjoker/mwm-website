using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.DefaultAdvert
{
    /// <summary>
    /// 預設廣告-Service Layer
    /// </summary>
    public interface IDefaultAdvertRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        Task<int> Add_Record(DefaultAdvertEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> Update_Record(DefaultAdvertEntity entity);

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        Task<int> Del_Record(int id,int types);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_str"></param>
        /// <param name="ads_type"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<(IEnumerable<DefaultAdvertEntity> record, long count)> GetRecords( string query_str,int ads_type, int index, int size);
    }
}