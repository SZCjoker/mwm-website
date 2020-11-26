using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.AdvertTrafficMaster
{
    /// <summary>
    /// 流量主後台-我的廣告
    /// </summary>
    public interface IAdvertTrafficMasterRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        Task<int> Add_Record(AdvertTrafficMasterEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        Task<int> Update_Record(AdvertTrafficMasterEntity entity, int account_id);

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        Task<int> Del_Record(int id,int types);

        /// <summary>
        /// 取列表
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="ads_type"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<(IEnumerable<AdvertTrafficMasterEntity> record, long count)> GetRecords( int account_id,int ads_type, int index, int size);
    }
}