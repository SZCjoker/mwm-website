using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenixnet.Extensions.Method
{
    public static class ListKeyValuePairExtension
    {
        public static string AppendFormPostData(this List<KeyValuePair<string, object>> data)
        {
            var result = string.Empty;
            for (var i = 0; i < data.Count; i++)
            {
                var l = data[i].Key;
                if (i == data.Count - 1)
                {
                    result += $"{data[i].Key}={data[i].Value}";
                }
                else
                {
                    result += $"{data[i].Key}={data[i].Value}&";
                }
            }

            return result;
        }
    }
}