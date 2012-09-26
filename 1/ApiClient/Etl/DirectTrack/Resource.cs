using System.Xml.Serialization;

namespace DirectTrack
{
    [XmlType(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/resourceList/v1_0")]
    public class Resource
    {
        [XmlAttribute(DataType = "anyURI", AttributeName = "location")]
        public string Location { get; set; }

        [XmlAttribute(AttributeName = "metaData1")]
        public string MetaData1 { get; set; }

        [XmlAttribute(AttributeName = "metaData2")]
        public string MetaData2 { get; set; }

        [XmlAttribute(AttributeName = "metaData3")]
        public string MetaData3 { get; set; }

        [XmlAttribute(AttributeName = "metaData4")]
        public string MetaData4 { get; set; }
    }
}
