using System;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Handler
{
    public static class MethodHandler
    {
        public static async Task<T> TryCatchAsync<T>(Func<Task<T>> func, Action<string> exceptionHandler)
        {
            try
            {
                return await func();
            }
            catch (AggregateException ex)
            {
                var innerEx = ex.InnerExceptions[0];

                if (innerEx is NotSupportedException)
                {
                    exceptionHandler(ex.ToString());
                    return default(T);
                }
                else if (innerEx is NotImplementedException)
                {
                    exceptionHandler(ex.ToString());
                    return default(T);
                }
                else
                {
                    exceptionHandler(ex.ToString());
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                exceptionHandler(ex.ToString());
                return default(T);
            }
        }

        public static async Task<T> TryCatchAsync<T>(Func<T, Task<T>> func, T funcParam, Action<string> exceptionHandler)
        {
            try
            {
                return await func(funcParam);
            }
            catch (AggregateException ex)
            {
                var innerEx = ex.InnerExceptions[0];

                if (innerEx is NotSupportedException)
                {
                    exceptionHandler(ex.ToString());
                    return default;
                }
                else if (innerEx is NotImplementedException)
                {
                    exceptionHandler(ex.ToString());
                    return default(T);
                }
                else
                {
                    exceptionHandler(ex.ToString());
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                exceptionHandler(ex.ToString());
                return default(T);
            }
        }
    }
}
