using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace LTWeb
{
    public class Lead : Entity<long>
    {
        public DateTime Timestamp { get; set; }

        public string RequestContent { get; set; }

        public string ResponseContent { get; set; }

        public DateTime ResponseTimestamp { get; set; }

        public string AppId { get; set; }

        public string AffiliateId { get; set; }

        public string IPAddress { get; set; }

        public int IsError { get; set; }

        [NotMapped]
        public XElement RequestContentXml
        {
            get { return XElement.Parse(RequestContent); }
            set { RequestContent = value.ToString(); }
        }

        [NotMapped]
        public XElement ResponseContentXml
        {
            get { return XElement.Parse(ResponseContent); }
            set { ResponseContent = value.ToString(); }
        }
    }
}
