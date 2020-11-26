using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Phoenixnet.Extensions.Data.MySql
{
    /// <summary>
    /// 資料庫連線工廠
    /// </summary>
    public interface IDbFactory
    {
        /// <summary>
        /// 建立資料庫連線
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        Task<MySqlConnection> CreateConnectionAsync(AccessMode policy = AccessMode.ReadOnly);

        /// <summary>
        /// 開啟資料庫連線
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        Task<MySqlConnection> OpenConnectionAsync(AccessMode policy = AccessMode.ReadOnly);

        /// <summary>
        /// 建立資料庫連線,由外部直接指定
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="connection">外部指定所使用的連線字串</param>
        /// <returns></returns>
        Task<MySqlConnection> OpenConnectionAsync(string setting);

        /// <summary>
        /// 建立資料庫連線
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        MySqlConnection CreateConnection(AccessMode policy = AccessMode.ReadOnly);

        /// <summary>
        /// 開啟資料庫連線
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        MySqlConnection OpenConnection(AccessMode policy = AccessMode.ReadOnly);


        
    }
}