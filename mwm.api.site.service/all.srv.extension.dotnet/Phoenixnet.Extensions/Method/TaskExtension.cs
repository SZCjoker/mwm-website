using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Method
{
    /// <summary>
    ///     <see cref="Task" /> 擴充函式。
    /// </summary>
    public static class TaskExtension
    {
        /// <summary>
        ///     在無 async/await 環境下替代 await 功能。
        /// </summary>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        public static void Await(this Task task, bool continueOnCapturedContext = false)
        {
            task.ConfigureAwait(continueOnCapturedContext)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        ///     在無 async/await 環境下替代 await 功能。
        /// </summary>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Await<T>(this Task<T> task, bool continueOnCapturedContext = false)
        {
            return task.ConfigureAwait(continueOnCapturedContext)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        ///     由ServiceStack Fork過來,主要是對IEnumerable來做非同步迭代的處理
        /// </summary>
        /// <param name="items"></param>
        /// <param name="fn"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task EachAsync<T>(this IEnumerable<T> items, Func<T, int, Task> fn)
        {
            var tcs = new TaskCompletionSource<object>();
            var enumerator = items.GetEnumerator();
            var i = 0;
            var next = (Action<Task>)null;
            next = t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerExceptions);
                else if (t.IsCanceled)
                    tcs.TrySetCanceled();
                else
                    StartNextIteration(tcs, fn, enumerator, ref i, next);
            };
            StartNextIteration(tcs, fn, enumerator, ref i, next);
            tcs.Task.ContinueWith(_ => enumerator.Dispose(), TaskContinuationOptions.ExecuteSynchronously);
            return tcs.Task;
        }

        private static void StartNextIteration<T>(TaskCompletionSource<object> tcs, Func<T, int, Task> fn, IEnumerator<T> enumerator, ref int i, Action<Task> next)
        {
            bool flag;
            try
            {
                flag = enumerator.MoveNext();
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
                return;
            }

            if (!flag)
            {
                tcs.SetResult(null);
            }
            else
            {
                var task = (Task)null;
                try
                {
                    task = fn(enumerator.Current, i);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }

                ++i;
                task?.ContinueWith(next, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }
        }
    }
}