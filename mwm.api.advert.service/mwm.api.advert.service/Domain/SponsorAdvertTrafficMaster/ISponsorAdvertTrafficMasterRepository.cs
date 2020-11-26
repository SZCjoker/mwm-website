using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Advert.Service.Domain.SponsorAdvertTrafficMaster
{
  /// <summary>
  /// 流量主-全民贊助廣告
  /// </summary>
  public  interface ISponsorAdvertTrafficMasterRepository
    {
        /// <summary>
        /// 取列表
        /// </summary>
        /// <param name="query_str"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<(IEnumerable<SponsorAdvertTrafficMasterEntity> record, long count)> GetRecords( string query_str, int index, int size);
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
        Task<int> Update_Record(SponsorAdvertTrafficMasterEntity entity);
        
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        Task<int> Add_Record(SponsorAdvertTrafficMasterEntity entity);
    } 

}