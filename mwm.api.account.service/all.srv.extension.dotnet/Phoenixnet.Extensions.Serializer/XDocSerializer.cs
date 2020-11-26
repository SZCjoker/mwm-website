using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Phoenixnet.Extensions.Serializer
{
    public class XDocSerializer : ISerializer
    {
        public T Deserialize<T>(string data)
        {
            return XDocSerializerUtils.Deserialize<T>(data);
        }

        public string Serialize<T>(T data)
        {
            XDocument doc = new XDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var writer = doc.CreateWriter())
            {
                xmlSerializer.Serialize(writer, data, ns);
            }

            return doc.ToString(SaveOptions.DisableFormatting).ToString();
        }

        public string Serialize(object data)
        {
            throw new NotImplementedException();
        }
    }

    public static class XDocSerializerUtils
    {
        public static T Deserialize<T>(string data)
        {
            XDocument doc = XDocument.Parse(data);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (var reader = doc.Root.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }
    }
}