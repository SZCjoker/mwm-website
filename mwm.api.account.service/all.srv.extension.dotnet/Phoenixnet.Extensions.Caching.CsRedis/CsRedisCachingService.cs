using CSRedis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Caching.CsRedis
{
    public class CsRedisCachingService : ICachingService
    { 
        public readonly CSRedisClient _manager;

        public CsRedisCachingService(CSRedisClient manger)
        {
            _manager = manger;
        }

        public T StartPipe<T>()
        {
            return (T)Convert.ChangeType(_manager.StartPipe(), typeof(T));

        } 

        public CSRedisClientPipe<string> StartPipe()
        {
            return _manager.StartPipe();
        }

        public bool Del(string key)
        {
            return (_manager.Del(key) > 0 ) ? true : false;
        }

        public async Task<bool> DelAsync(string key)
        {
            return (await _manager.DelAsync(key) > 0) ? true : false;
        }

        public bool Exist(string key)
        {
            return _manager.Exists(key);
        }

        public async Task<bool> ExistAsync(string key)
        {
            return await _manager.ExistsAsync(key);
        }

        public string Get(string key)
        {
            return _manager.Get(key);
        }

        public async Task<string> GetAsync(string key)
        {
            return await _manager.GetAsync(key);
        }

        public bool HashExist(string key, string field)
        {
            return _manager.HExists(key, field);
        }

        public bool HDel(string key, string field)
        {
            return (_manager.HDel(key) > 0) ? true : false;
        }

        public async Task<bool> HDelAsync(string key, string field)
        { 
            return (await _manager.HDelAsync(key, field) > 0) ? true : false;
        }

        public async Task<bool> HExistAsync(string key, string field)
        {
            return await _manager.HExistsAsync(key, field);
        }

        public string HGet(string key, string field)
        {
            return _manager.HGet(key, field);
        }

        public Dictionary<string, string> HGetAll(string key)
        {
            return _manager.HGetAll(key);
        }

        public async Task<Dictionary<string, string>> HGetAllAsync(string key)
        {
            return await _manager.HGetAllAsync(key);
        }

        public async Task<string> HGetAsync(string key, string field)
        {
            return await _manager.HGetAsync(key, field);
        }

        public long HIncrBy(string key, string field, long value = 0)
        {
            if (value == 0)
                return _manager.HIncrBy(key, field);
            else
                return _manager.HIncrBy(key, field, value);
        }

        public async Task<long> HIncrByAsync(string key, string field, long value = 0)
        {
            if (value == 0)
                return await _manager.HIncrByAsync(key, field);
            else
                return await _manager.HIncrByAsync(key, field, value);
        }

        public bool HSet(string key, string field, object value)
        {
            return _manager.HSet(key, field, value);            
        }

        public async Task<bool> HSetAsync(string key, string field, object value)
        {
            return await _manager.HSetAsync(key, field, value);
        }

        public long IncrBy(string key)
        {
            return _manager.IncrBy(key);
        }

        public Task<long> IncrByAsync(string key)
        {
            return _manager.IncrByAsync(key);
        }

        public bool Set(string key, string value)
        {
            return _manager.Set(key, value);
        }

        public async Task<bool> SetAsync(string key, object value)
        {
            return await _manager.SetAsync(key, value);
        }

        public bool Expire(string key, TimeSpan time)
        {
            return _manager.Expire(key, time);
        }

        public async Task<bool> ExpireAsync(string key, TimeSpan time)
        {
            return await _manager.ExpireAsync(key, time);
        }

        public bool ExpireAt(string key, DateTime time)
        {
            return _manager.ExpireAt(key, time);
        }

        public async Task<bool> ExpireAtAsync(string key, DateTime time)
        {
            return await _manager.ExpireAtAsync(key, time);
        }
 
    }
}
