using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Data
{
    public static class DbDataReaderExtension
    {
        /// <summary>
        /// 讀取資料為 <see cref="List{T}"/>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="block"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ReadToList<T>(this DbDataReader reader, Func<DbDataReader, T> block)
        {
            var list = new List<T>();

            while (reader.Read())
            {
                list.Add(block.Invoke(reader));
            }

            return list;
        }

        /// <summary>
        /// 讀取資料為 <see cref="List{T}"/>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="block"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> ReadToListAsync<T>(this DbDataReader reader, Func<DbDataReader, T> block)
        {
            var list = new List<T>();

            while (await reader.ReadAsync())
            {
                list.Add(block.Invoke(reader));
            }

            return list;
        }

        /// <summary>
        /// 讀取資料
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="block"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ReadSingle<T>(this DbDataReader reader, Func<DbDataReader, T> block)
        {
            if (reader.Read())
            {
                return block.Invoke(reader);
            }

            return default(T);
        }

        /// <summary>
        /// 讀取資料
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="block"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> ReadSingleAsync<T>(this DbDataReader reader, Func<DbDataReader, T> block)
        {
            if (await reader.ReadAsync())
            {
                return block.Invoke(reader);
            }

            return default(T);
        }
    }
}