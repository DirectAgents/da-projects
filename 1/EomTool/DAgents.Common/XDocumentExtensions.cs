using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DAgents.Common
{
    public static class XDocumentExtensions
    {
        public static T Deserialize<T>(this XDocument source)
        {
            var serializer = new XmlSerializer(typeof(T));
            var reader = source.CreateReader();
            if (serializer.CanDeserialize(reader))
                return (T)serializer.Deserialize(reader);
            else
                throw new Exception("can't deserialize " + typeof(T).FullName);
        }

        public static string Serialize<T>(this XDocument source, T obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            StringBuilder sb = new StringBuilder();
            var sw = new StringWriter(sb);
            serializer.Serialize(sw, obj);
            return sb.ToString();
        }
    }
}
