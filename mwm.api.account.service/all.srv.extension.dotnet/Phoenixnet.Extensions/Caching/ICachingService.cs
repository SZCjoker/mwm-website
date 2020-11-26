using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Caching
{
    public interface ICachingService
    { 
        T StartPipe<T>(); 

        /// <summary>
        /// 寫入 Redis
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Set(string key, string value);

        Task<bool> SetAsync(string key, object value);

        bool HSet(string key, string field, object value);

        Task<bool> HSetAsync(string key, string field, object value);

        /// <summary>
        /// 寫入 Redis (有時效性)
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="json"></param>
        /// <returns></returns>
        bool Expire(string key, TimeSpan time);

        Task<bool> ExpireAsync(string key, TimeSpan time);

        bool ExpireAt(string key, DateTime time);

        Task<bool> ExpireAtAsync(string key, DateTime time);

        /// <summary>
        /// 取得 Redis 內容
        /// </summary>
        /// <param name="key">KEY</param>
        /// <returns></returns>
        string Get(string key);

        Task<string> GetAsync(string key);

        string HGet(string key, string field);

        Task<string> HGetAsync(string key, string field);

        Dictionary<string, string> HGetAll(string key);

        Task<Dictionary<string,string>> HGetAllAsync(string key);

        /// <summary>
        /// 由鍵值刪除資料
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Del(string key);
        Task<bool> DelAsync(string key);

        bool HDel(string key, string field);

        Task<bool> HDelAsync(string key, string field);

        /// <summary>
        /// 取流水號 (Async)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long IncrBy(string key);

        long HIncrBy(string key, string field, long value = 0);

        Task<long> IncrByAsync(string key);

        Task<long> HIncrByAsync(string key, string field, long value = 0);

        /// <summary>
        /// 鍵值是否存在
        /// </summary>
        /// <param name="key">KEY</param>
        /// <returns></returns>
        bool Exist(string key);

        bool HashExist(string key, string field);

        Task<bool> ExistAsync(string key);

        Task<bool> HExistAsync(string key, string field);
 
    }
}