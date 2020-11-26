using System;
using System.Collections.Generic;
using System.Linq;

namespace Phoenixnet.Extensions.Method
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 取得列舉值的Attribute
        /// </summary>
        /// <typeparam name="T">Attribute類型</typeparam>
        /// <param name="obj">列舉值</param>
        /// <returns>Attribute類型</returns>
        public static T GetEnumAttribute<T>(this object obj)
            where T : System.Attribute
        {
            Type type = obj.GetType();

            if (!type.IsEnum)
                throw new InvalidCastException("T must be an enum type");

            var member = type
                .GetMembers()
                .FirstOrDefault(m => m.Name.Equals(obj.ToString()));

            var attr = member.GetCustomAttributes(typeof(T), true)
                .FirstOrDefault();

            return (T)attr;
        }

        /// <summary>
        /// 資料轉換處理
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <returns></returns>
        public static R Let<T, R>(this T obj, Func<T, R> func)
        {
            return func.Invoke(obj);
        }

        /// <summary>
        /// 物件轉換為Form Post Data 並做降冪排序
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ClassToFormPostParamAsc(this object instance)
        {
            var dict = instance.SortPropertyAsc();
            return dict.AppendFormPostData();
        }

        /// <summary>
        /// 物件轉換為Form Post Data 並做升冪排序
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ClassToFormPostParamDesc(this object instance)
        {
            var dict = instance.SortPropertyDesc();
            return dict.AppendFormPostData();
        }

        private static List<KeyValuePair<string, object>> SortPropertyAsc(this object instance)
        {
            return instance.GetType().GetProperties()
                .Select(property => new KeyValuePair<string, object>(property.Name, property.GetValue(instance)))
                .OrderBy(s => s.Key).ToList();
        }

        private static List<KeyValuePair<string, object>> SortPropertyDesc(this object instance)
        {
            return instance.GetType().GetProperties()
                .Select(property => new KeyValuePair<string, object>(property.Name, property.GetValue(instance)))
                .OrderByDescending(s => s.Key).ToList();
        }
    }
}