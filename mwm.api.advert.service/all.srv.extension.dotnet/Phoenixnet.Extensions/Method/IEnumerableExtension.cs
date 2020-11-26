using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Phoenixnet.Extensions.Method
{
    /// <summary>IEnumerable 擴充類別
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>根據傳入的 Lambda 運算式 對指定的屬性去除重複的資料
        /// </summary>
        /// <typeparam name="TSource">資料集合泛型</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source">資料集合</param>
        /// <param name="selector">資料欄位篩選 Lambda 運算式</param>
        /// <returns>去除重複資料的資料集合</returns>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            var keys = new HashSet<TKey>();

            foreach (TSource m in source)
            {
                var value = selector(m);

                if (keys.Add(value))
                {
                    yield return m;
                }
            }
        }

        /// <summary>檢查集合中是否有滿足條件的重覆的元素<para/>
        /// 範例:<para/>
        /// var source1 = new List&lt;string&gt;() { "A", "B", "C", "C" };<para/>
        /// var result1 = source1.HasDuplicates( q => q );<para/>
        /// Assert.IsTrue( result1 );<para/>
        /// var source2 = new List&lt;UserModel&gt;() { ... };<para/>
        /// var result2 = source2.HasDuplicates( q => q.Name );<para/>
        /// Assert.IsTrue( result2 );<para/>
        /// var source4 = new string[] { "1", "2", "A", "BBB" };<para/>
        /// var result4 = source4.HasDuplicates( s => s );<para/>
        /// Assert.IsFalse( result4 );<para/>
        /// </summary>
        /// <example>
        /// <code>
        /// var source1 = new List&lt;string&gt;() { "A", "B", "C", "C" };
        /// var result1 = source1.HasDuplicates( q => q );
        /// Assert.IsTrue( result1 );
        /// var source2 = new List&lt;UserModel&gt;() { ... };
        /// var result2 = source2.HasDuplicates( q => q.Name );
        /// Assert.IsTrue( result2 );
        /// var source4 = new string[] { "1", "2", "A", "BBB" };
        /// var result4 = source4.HasDuplicates( s => s );
        /// Assert.IsFalse( result4 );
        /// </code>
        /// </example>
        /// <param name="source"></param>
        /// <param name="selector">資料欄位篩選委派</param>
        /// <typeparam name="T">元素的泛型</typeparam>
        /// <typeparam name="TKey">資料欄位的泛型</typeparam>
        /// <returns>是否有滿足條件的重覆的元素</returns>
        public static bool HasDuplicates<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
        {
            var distinctCount = source.Distinct<T, TKey>(selector).Count();

            if (distinctCount < source.Count())
            {
                return true;
            }

            return false;
        }

        /// <summary>取得集合中的元素數量
        /// </summary>
        /// <param name="source"></param>
        /// <returns>集合中的元素數量</returns>
        public static int Count(this IEnumerable source)
        {
            if (null == source)
            {
                return 0;
            }

            int count = 0;

            foreach (var item in source)
            {
                count++;
            }

            return count;
        }

        /// <summary>是否指向Null或一個長度為 0 的空集合
        /// </summary>
        /// <param name="source">物件集合</param>
        /// <returns>是否指向Null或一個長度為 0 的空集合</returns>
        public static bool IsNullOrEmpty(this IEnumerable source)
        {
            if (null == source || 0 == source.Count())
            {
                return true;
            }

            return false;
        }

        /// <summary>是否不指向 Null 而且長度不為 0 的集合
        /// </summary>
        /// <param name="source">物件集合</param>
        /// <returns>是否不指向 Null 而且長度不為 0 的集合</returns>
        public static bool IsNotNullAndEmpty(this IEnumerable source)
        {
            if (null == source)
            {
                return false;
            }

            if (0 == source.Count())
            {
                return false;
            }

            return true;
        }

        public static IEnumerable<TSource> WhereNot<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    TSource element = e.Current;
                    if (!predicate(element))
                        yield return element;
                }
            }
        }

        public static bool IsEmpty<T>(this IEnumerable<T> source) => source.IsEmpty(null);

        public static bool IsEmpty<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null) return true;
            return !(predicate == null ? source.Any() : source.Any(predicate));
        }
    }
}