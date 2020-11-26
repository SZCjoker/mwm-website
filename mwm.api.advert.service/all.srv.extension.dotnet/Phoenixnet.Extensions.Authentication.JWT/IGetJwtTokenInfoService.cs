using System.Collections.Generic;

namespace  Phoenixnet.Extensions.Authentication.JWT
{
    /// <summary>取得JWT使用者資訊,須先注入IHttpContextAccessor</summary>
    public interface IGetJwtTokenInfoService
    {
        /// <summary>
        /// 取得JWT內的使用者ID
        /// </summary>
        int UserId { get;  }
        /// <summary>
        /// 取得JWT內的使用者登入名稱
        /// </summary>
        string LoginName { get;  }
        /// <summary>
        /// 取得JWT內的使用者權限
        /// </summary>
        IEnumerable<string> Permission { get;  }
    }
}