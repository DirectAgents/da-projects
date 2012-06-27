using System.IO;
using System.Xml.Serialization;
namespace DirectTrack.Rest
{
    public class RestEntity<T>
    {
        public RestEntity(T inner)
        {
            this.inner = inner;
        }

        public RestEntity(string xml)
        {
            this.inner = DeserializeXml(xml);
        }

        protected T inner;

        private static T DeserializeXml(string xml)
        {
            var xs = new XmlSerializer(typeof(T));
            return (T)(xs.Deserialize(new StringReader(xml)));
        }
    }
}
