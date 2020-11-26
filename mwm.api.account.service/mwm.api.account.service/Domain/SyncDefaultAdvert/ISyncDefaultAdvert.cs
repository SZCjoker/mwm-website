using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Domain.SyncDefaultAdvert
{
    /// <summary>
    /// 同步預設廣告
    /// </summary>
    public interface ISyncDefaultAdvert
    {
        /// <summary>
        /// 同步預設廣告
        /// </summary>
        /// <returns></returns>
        Task<int> SyncRecords(int account_id);
    }
}