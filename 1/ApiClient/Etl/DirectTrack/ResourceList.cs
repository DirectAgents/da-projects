using System.Xml.Serialization;

namespace DirectTrack
{
    [XmlType(AnonymousType = true, Namespace = "http://www.digitalriver.com/directtrack/api/resourceList/v1_0")]
    [XmlRoot(Namespace = "http://www.digitalriver.com/directtrack/api/resourceList/v1_0", IsNullable = false, ElementName = "resourceList")]
    public class ResourceList
    {
        [XmlElement("resourceURL")]
        public Resource[] Resources { get; set; }

        [XmlAttribute(DataType = "anyURI", AttributeName = "location")]
        public string Location { get; set; }

        public bool HasResources { get { return this.Resources != null && this.Resources.Length > 0; } }
    }
}
