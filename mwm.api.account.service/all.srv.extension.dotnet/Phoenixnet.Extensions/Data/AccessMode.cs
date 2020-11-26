namespace Phoenixnet.Extensions.Data
{
    /// <summary>
    /// 資料庫連線類型
    /// </summary>
    public enum AccessMode : byte
    {
        /// <summary>
        /// 唯讀
        /// </summary>
        ReadOnly = 0,

        /// <summary>
        /// 可讀寫
        /// </summary>
        ReadWrite = 1
    }
}