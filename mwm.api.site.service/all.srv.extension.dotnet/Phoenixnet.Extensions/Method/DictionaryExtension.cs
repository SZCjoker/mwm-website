using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phoenixnet.Extensions.Method
{
    public static class DictionaryExtension
    {
        public static string ToJson(this Dictionary<string, string> dict)
        {
            var entries = dict.Select(d =>
                string.Format("\"{0}\": \"{1}\"", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", entries) + "}";
        }

        public static string ToText(this Dictionary<string, string> dict)
        {
            return dict.Select(x => x.Key + "=" + x.Value).Aggregate((s1, s2) => s1 + ";" + s2);
        }
    }
}
