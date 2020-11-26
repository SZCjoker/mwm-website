using System;
using System.Collections.Generic;
using System.Linq;

namespace Phoenixnet.Extensions.Method
{
    /// <summary>
    ///     <see cref="Enumerable" /> 擴充函式
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        ///     分割列表為固定大小子列表
        /// </summary>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<List<T>> SplitList<T>(this List<T> source, int size)
        {
            if (size < 1) yield return source;

            for (var i = 0; i < source.Count; i += size) yield return source.GetRange(i, Math.Min(size, source.Count - i));
        }

        /// <summary>
        /// 多載（1）由ServiceStack Fork過來,主要是對IEnumerable來做迭代的處理
        /// </summary>
        /// <param name="values"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public static void Each<T>(this IEnumerable<T> values, Action<T> action)
        {
            if (values == null)
                return;
            foreach (var obj in values)
                action(obj);
        }

        /// <summary>
        ///多載（2） 由ServiceStack Fork過來,主要是對IEnumerable來做迭代的處理
        /// </summary>
        /// <param name="values"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public static void Each<T>(this IEnumerable<T> values, Action<int, T> action)
        {
            if (values == null)
                return;
            var num = 0;
            foreach (var obj in values)
                action(num++, obj);
        }

        /// <summary>
        ///  多載（3）由ServiceStack Fork過來,主要是對IEnumerable來做迭代的處理
        /// </summary>
        /// <param name="map"></param>
        /// <param name="action"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        public static void Each<TKey, TValue>(this IDictionary<TKey, TValue> map, Action<TKey, TValue> action)
        {
            if (map == null)
                return;
            foreach (var index in map.Keys.ToList())
                action(index, map[index]);
        }
    }
}