using System.IO;
using System.Xml.Serialization;

namespace QuickBooks
{
    public partial class MetaData
    {
        // NOTE: QBMetaData.xml is assumed to be in current directory
        public static MetaData Create()
        {
            return Create("QBMetaData.xml");
        }

        public static MetaData Create(string path)
        {
            MetaData instance;
            using (var stream = new StreamReader(path))
            {
                var serializer = new XmlSerializer(typeof(MetaData));
                instance = (MetaData)serializer.Deserialize(stream);
            }
            return instance;
        }
    }
}
