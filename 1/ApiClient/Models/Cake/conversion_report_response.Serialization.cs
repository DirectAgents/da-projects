//------------------------------------------------------------------------------
// <auto-generated>
//     This source code was auto-generated by XsdClassGen.tt.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.IO;
using System.Xml;
using System.Xml.Serialization;

// AA: added
using Extensions;

namespace ApiClient.Models.Cake
{
	public partial class conversion_report_response
	{
		// AA: added
		public static conversion_report_response DeserializeFrom(string content)
		{
            using (var stream = content.ToStream())
				return (conversion_report_response)DeserializeFrom(stream);
		}

		public static conversion_report_response DeserializeFrom(Stream stream)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(conversion_report_response));
			return (conversion_report_response)serializer.Deserialize(stream);
		}

		public static conversion_report_response DeserializeFrom(TextReader reader)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(conversion_report_response));
			return (conversion_report_response)serializer.Deserialize(reader);
		}

		public static conversion_report_response DeserializeFrom(XmlReader reader)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(conversion_report_response));
			return (conversion_report_response)serializer.Deserialize(reader);
		}

		public void SerializeTo(Stream stream)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(conversion_report_response));
			serializer.Serialize(stream, this);
		}

		public void SerializeTo(TextWriter writer)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(conversion_report_response));
			serializer.Serialize(writer, this);
		}

		public void SerializeTo(XmlWriter writer)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(conversion_report_response));
			serializer.Serialize(writer, this);
		}
	}
}
