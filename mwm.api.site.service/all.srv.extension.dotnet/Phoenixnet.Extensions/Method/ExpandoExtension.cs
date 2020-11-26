using System.Collections.Generic;
using System.Dynamic;

namespace Phoenixnet.Extensions.Method
{
    public static class ExpandoExtension
    {
        public static bool ContainKey(ExpandoObject obj, string key)
        {
            return ((IDictionary<string, object>)obj).ContainsKey(key);
        }
    }
}