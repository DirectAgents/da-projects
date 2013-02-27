using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace KimberlyClark.Services.Concrete
{
    public static class Utilities
    {
        public static string ToXmlString<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            serializer.Serialize(sw, obj);
            sw.Close();
            var doc = XDocument.Parse(sb.ToString());
            var xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            if (doc.Root != null)
                doc.Root.Elements().Where(c => c.Attribute(xsi + "nil") != null).ToList().ForEach(c => c.Remove());
            return doc.ToString();
        }
    }
}
