using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Phoenixnet.Extensions.Serializer.Newtonsoft
{
    public interface IParamSerializer
    {
        string Serialize(object obj);

        object Deserialize(string input);

        T Deserialize<T>(string input);

        object Deserialize(NameValueCollection input);

        T Deserialize<T>(NameValueCollection input);
    }
}