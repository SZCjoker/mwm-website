using System;
using System.Text;
using Phoenixnet.Extensions.Method;
using ZeroFormatter;

namespace Phoenixnet.Extensions.Serializer
{
    public class ZeroSerializer : ISerializer
    {
        public T Deserialize<T>(string data)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            return ZeroFormatterSerializer.Deserialize<T>(byteArray);
        }

        /// <summary>
        ///
        /// var byts = text.ToByteArryFromHex();
        /// var data1 = ZeroFormatterSerializer.Deserialize<T>(byteArray);
        /// var data2 = ZeroFormatterSerializer.Deserialize<T>(byts);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Serialize<T>(T data)
        {
            byte[] byteArray = ZeroFormatterSerializer.Serialize(data);
            return byteArray.ToHex();
        }

        public string Serialize(object data)
        {
            throw new NotImplementedException();
        }
    }
}