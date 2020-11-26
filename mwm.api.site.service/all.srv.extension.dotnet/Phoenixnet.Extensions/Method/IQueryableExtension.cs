using System;
using System.Linq;
using System.Linq.Expressions;

namespace Phoenixnet.Extensions.Method
{
    public static partial class IQueryableExtension
    {
        public static IQueryable<TSource> WhereNot<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var exp = Expression.Call(typeof(Queryable),
                                      "Where",
                                      new Type[] { source.ElementType },
                                      source.Expression,
                                      Expression.Lambda<Func<TSource, bool>>(Expression.Not(predicate.Body), predicate.Parameters));

            return source.Provider.CreateQuery<TSource>(exp);
        }
    }
}