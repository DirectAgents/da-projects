using System.IO;
using System.Xml.Serialization;
using LTWeb.Common;

namespace LTWeb
{
    public class ServiceConfig : Entity<long>
    {
        public SourceOfRequestType SourceOfRequest { get; set; }

        public string PostUrl { get; set; }

        public string Name { get; set; }
    }
}
