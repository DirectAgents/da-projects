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
	public partial class ArrayOfDailySummary
	{
		// AA: added
		public static ArrayOfDailySummary DeserializeFrom(string content)
		{
            using (var stream = content.ToStream())
				return (ArrayOfDailySummary)DeserializeFrom(stream);
		}

		public static ArrayOfDailySummary DeserializeFrom(Stream stream)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfDailySummary));
			return (ArrayOfDailySummary)serializer.Deserialize(stream);
		}

		public static ArrayOfDailySummary DeserializeFrom(TextReader reader)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfDailySummary));
			return (ArrayOfDailySummary)serializer.Deserialize(reader);
		}

		public static ArrayOfDailySummary DeserializeFrom(XmlReader reader)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfDailySummary));
			return (ArrayOfDailySummary)serializer.Deserialize(reader);
		}

		public void SerializeTo(Stream stream)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfDailySummary));
			serializer.Serialize(stream, this);
		}

		public void SerializeTo(TextWriter writer)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfDailySummary));
			serializer.Serialize(writer, this);
		}

		public void SerializeTo(XmlWriter writer)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfDailySummary));
			serializer.Serialize(writer, this);
		}
	}
}
