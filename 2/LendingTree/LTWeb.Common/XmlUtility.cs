using System.IO;
using System.Xml.Serialization;

namespace LTWeb.Common
{
    public static class XmlUtility
    {
        public static T Deserialize<T>(string xml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(xml))
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }
    }
}
