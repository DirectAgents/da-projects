//------------------------------------------------------------------------------
// <auto-generated>
//     This source code was auto-generated by XsdClassGen.tt.
//     Runtime Version:4.0.30319.269
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

namespace ApiClient.Models
{
	public partial class advertiser
	{
		// AA: added
		public static advertiser DeserializeFrom(string content)
		{
            using (var stream = content.ToStream())
				return (advertiser)DeserializeFrom(stream);
		}

		public static advertiser DeserializeFrom(Stream stream)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(advertiser));
			return (advertiser)serializer.Deserialize(stream);
		}

		public static advertiser DeserializeFrom(TextReader reader)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(advertiser));
			return (advertiser)serializer.Deserialize(reader);
		}

		public static advertiser DeserializeFrom(XmlReader reader)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(advertiser));
			return (advertiser)serializer.Deserialize(reader);
		}

		public void SerializeTo(Stream stream)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(advertiser));
			serializer.Serialize(stream, this);
		}

		public void SerializeTo(TextWriter writer)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(advertiser));
			serializer.Serialize(writer, this);
		}

		public void SerializeTo(XmlWriter writer)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(advertiser));
			serializer.Serialize(writer, this);
		}
	}
}
