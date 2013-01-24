using System.IO;
using System.Xml.Serialization;

namespace QuickBooks.Metadata
{
    public partial class QBMetaData
    {
        // NOTE: if you call this factory method it is assumed that you've
        //       copied QBMetaData.xml to the output directory
        public static QBMetaData Create()
        {
            return Create("QBMetaData.xml");
        }

        public static QBMetaData Create(string path)
        {
            QBMetaData instance;
            using (var stream = new StreamReader(path))
            {
                var serializer = new XmlSerializer(typeof(QBMetaData));
                instance = (QBMetaData)serializer.Deserialize(stream);
            }
            return instance;
        }
    }
}
