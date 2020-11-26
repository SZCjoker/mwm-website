using System.Collections.Generic;
using Phoenixnet.Extensions.Object;

namespace Phoenixnet.Extensions.Handler
{
    public static class StateCodeHandler
    {
        public static BasicResponse ForBool(bool result, string success = "success", string failed = "failed")
        {
            if (result)
                return new BasicResponse {desc = success, code = 200};
            return new BasicResponse {desc = failed, code = 300};
        }

        public static BasicResponse<T> ForBool<T>(bool result, T data, string success = "success", string failed = "failed")
        {
            if (result)
                return new BasicResponse<T> {desc = success, code = 200, data = data};
            return new BasicResponse<T> {desc = failed, code = 300, data = default};
        }

        public static BasicResponse ForCount(int count, string success = "success", string failed = "failed")
        {
            if (count > 0)
                return new BasicResponse {desc = success, code = 200};
            return new BasicResponse {desc = failed, code = 300};
        }

        public static BasicResponse<T> ForCount<T>(int count, T data, string success = "success", string failed = "failed")
        {
            if (count > 0)
                return new BasicResponse<T> {desc = success, code = 200, data = data};
            return new BasicResponse<T> {desc = failed, code = 300, data = default};
        }

        public static PagingResponse<T> ForPagingCount<T>(int count, T data, Paging paging = default, string success = "success", string failed = "failed")
        {
            if (count > 0)
                return new PagingResponse<T> { desc = success, code = 200, data = data, paging = paging };
            return new PagingResponse<T> { desc = failed, code = 300, data = default };
        }

        public static BasicResponse<T> ForNull<T>(T data, string success = "success", string failed = "failed")
        {
            if (data != null)
                return new BasicResponse<T> {desc = success, code = 200, data = data};
            return new BasicResponse<T> {desc = failed, code = 300, data = default};
        }

        public static BasicResponse<T> ForRecord<T>(T data, string success = "success", string failed = "failed")
        {
            return data != null ? new BasicResponse<T> {desc = success, code = 200, data = data} : new BasicResponse<T> {desc = failed, code = 300, data = default};
        }

        public static BasicResponse For(string desc, int code)
        {
            return new BasicResponse {desc = desc, code = code};
        }
        /// <summary>
        /// 分頁輸出
        /// </summary>
        /// <param name="data"></param>
        /// <param name="paging"></param>
        /// <param name="success"></param>
        /// <param name="failed"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IPagingResult<T> PagingRecord<T>(IEnumerable<T> data, Paging paging, string success = "success", string failed = "failed")
        {
            return data != null ? new ResultPaging<T>(data, paging, 200, success) : new ResultPaging<T>(default, paging, 300, failed);
        }
    }
}