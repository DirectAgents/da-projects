using System.IO;
using System.Xml.Serialization;

namespace LTWeb
{
    public class ServiceConfig : Entity<long>
    {
        public SourceOfRequestType SourceOfRequest { get; set; }

        public string PostUrl { get; set; }

        public string Name { get; set; }

        public static ServiceConfig CreateFromXml(string xml)
        {
            return Deserialize<ServiceConfig>(xml);
        }

        static T Deserialize<T>(string xml)
        {
            using (var reader = new StringReader(xml))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            }
        }
    }
}
