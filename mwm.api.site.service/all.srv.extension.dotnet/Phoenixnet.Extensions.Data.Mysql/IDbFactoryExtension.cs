using System;
using System.Data;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.Data.MySql
{
    public static class IDbFactoryExtension
    {
        public static async Task OperateAsync(this IDbFactory dbFactory, Func<IDbConnection, Task> function , AccessMode mode = AccessMode.ReadWrite)
        {
            using (IDbConnection conn = await dbFactory.OpenConnectionAsync(mode))
            {
                await function(conn);
            }
        }

        public static async Task<T> OperateAsync<T>(this IDbFactory dbFactory, Func<IDbConnection, Task<T>> function, AccessMode mode = AccessMode.ReadWrite )
        {
            using (IDbConnection conn = await dbFactory.OpenConnectionAsync(mode))
            {
                return await function(conn);
            }
        }

        public static async Task OperateAsync(this IDbFactory dbFactory, Func<IDbConnection, Task> function, string connection)
        {
            using (IDbConnection conn = await dbFactory.OpenConnectionAsync(connection))
            {
                await function(conn);
            }
        }

        public static async Task<T> OperateAsync<T>(this IDbFactory dbFactory, Func<IDbConnection, Task<T>> function, string connection)
        {
            using (IDbConnection conn = await dbFactory.OpenConnectionAsync(connection))
            {
                return await function(conn);
            }
        }
    }
}
