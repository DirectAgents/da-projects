using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DirectTrackXsdUtility
{
    static class XmlNamespaces
    {
        public static XNamespace XMLSchema = @"http://www.w3.org/2001/XMLSchema";
    }

    class SchemaElement
    {
        private string type = null;

        public SchemaElement(XsdFile xsdFile, XElement element)
        {
            this.Element = element;
            this.Parent = xsdFile;
        }

        public string Name
        {
            get
            {
                string name = this.Element.Attribute("name").Value;

                return name;
            }
        }

        public string Type
        {
            get
            {
                if (this.type == null)
                {
                    this.type = "unknown";
                    XAttribute typeAttribute = this.Element.Attribute("type");
                    if (typeAttribute != null)
                    {
                        this.type = typeAttribute.Value;
                    }
                    else
                    {
                        XElement simpleType = this.Element.Descendants(XmlNamespaces.XMLSchema + "simpleType").FirstOrDefault();
                        if (simpleType != null)
                        {
                            XElement restriction = simpleType.Descendants(XmlNamespaces.XMLSchema + "restriction").FirstOrDefault();
                            if (restriction != null)
                            {
                                XAttribute baseAttribute = restriction.Attribute("base");
                                if (baseAttribute != null)
                                {
                                    this.type = baseAttribute.Value;
                                }

                                XElement maxLength = restriction.Descendants(XmlNamespaces.XMLSchema + "maxLength").FirstOrDefault();
                                if (maxLength != null)
                                {
                                    this.MaxLenth = int.Parse(maxLength.Attribute("value").Value);
                                }
                            }
                            else
                            {
                                XElement complexType = this.Element.Descendants(XmlNamespaces.XMLSchema + "complexType").FirstOrDefault();
                                if (complexType != null)
                                {
                                    // deal with complex types
                                    this.type = "complex";

                                    XElement attribute = complexType.Descendants(XmlNamespaces.XMLSchema + "attribute").FirstOrDefault();
                                    if (attribute != null)
                                    {
                                        // deal with "name"
                                        // handle "type"
                                    }
                                }
                            }
                        }
                    }
                }
                return this.type;
            }
        }

        public XElement Element { get; set; }

        public int? MaxLenth { get; set; }

        public XsdFile Parent { get; set; }
    }

    class XsdFile
    {
        public XsdFile(string path, string primaryElement)
        {
            this.PrimaryElementName = primaryElement;
            this.Document = XDocument.Load(path);
        }

        public SchemaElement GetSchemaElement(string elementName)
        {
            var schemaElement =
                from c in this.SchemaElement.Elements(XmlNamespaces.XMLSchema + "element")
                where c.Attribute("name").Value == elementName
                select c;

            return new SchemaElement(this, schemaElement.First());
        }

        public XElement SchemaElement
        {
            get
            {
                var schemaElement = this.Document.Element(XmlNamespaces.XMLSchema + "schema");

                return schemaElement;
            }
        }

        public XElement PrimaryElement
        {
            get
            {
                var primaryElement =
                    from c in this.SchemaElement.Elements(XmlNamespaces.XMLSchema + "element")
                    where c.Attribute("name").Value == this.PrimaryElementName
                    select c;

                return primaryElement.First();
            }
        }

        public XDocument Document { get; set; }

        public string PrimaryElementName { get; set; }

        public IEnumerable<SchemaElement> PrimaryElementSchemaElements
        {
            get
            {
                XElement sequence = this.PrimaryElement
                    .Element(XmlNamespaces.XMLSchema + "complexType")
                    .Element(XmlNamespaces.XMLSchema + "sequence");

                IEnumerable<string> refs = sequence
                    .Elements(XmlNamespaces.XMLSchema + "element")
                    .Select(c => c.Attribute("ref").Value);

                foreach (string item in refs)
                {
                    yield return this.GetSchemaElement(item);
                }
            }
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //var xsdFile = new XsdFile(@"C:\AB\AccountingBackupWeb.Models\DirectTrack\Rest\Schemas\Affiliates\affiliate.xsd", "affiliate");
            //var xsdFile = new XsdFile(@"C:\AB\AccountingBackupWeb.Models\DirectTrack\Rest\Schemas\Payouts\payout.xsd", "payout");
            var xsdFile = new XsdFile(@"C:\GitHub\da-projects\1\ApiClient.Models.DirectTrack\Schemas\campaign.xsd", "campaign");

            var view = new View(xsdFile);
            Clipboard.SetText(view.Sql);
            Console.WriteLine("clipboard set");
        }
    }

    class View
    {
        public View(XsdFile xsdFile)
        {
            this.XsdFile = xsdFile;
        }

        public string Sql
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("WITH XMLNAMESPACES('http://www.digitalriver.com/directtrack/api/{0}/v1_0' AS x)\n", this.Element);
                sb.Append("SELECT\n");

                bool comma = false;
                foreach (var item in this.XsdFile.PrimaryElementSchemaElements
                    .Where(c=>c.Type != "unknown"))
                {
                    ViewColumn viewColumn = new ViewColumn(item);
                    sb.Append("\t");
                    if (comma)
                    {
                        sb.Append(",");
                    }
                    else
                    {
                        comma = true;
                        sb.Append(" ");
                    }
                    sb.AppendLine(viewColumn.SelectValueSql);
                }

                foreach (var item in this.XsdFile.PrimaryElementSchemaElements
                    .Where(c => c.Type == "unknown"))
                {
                    ViewColumn viewColumn = new ViewColumn(item);
                    sb.Append("\t");
                    if (comma)
                    {
                        sb.Append(",");
                    }
                    else
                    {
                        comma = true;
                        sb.Append(" ");
                    }
                    sb.AppendLine(viewColumn.SelectQuerySql);
                }

                sb.Append("\n");
                sb.Append("FROM [dbo].[DirectTrackResources]\n");
                sb.Append("WHERE ");
                sb.AppendFormat("[Name].exist('(/x:{0})[1]')=1\n", this.Element);

                return sb.ToString()
                    .Replace("\nFROM", "FROM");
            }
        }

        public XsdFile XsdFile { get; set; }

        public string Element
        {
            get
            {
                return this.XsdFile.PrimaryElementName;
            }
        }
    }

    class ViewColumn
    {
        private SchemaElement item;

        public ViewColumn(SchemaElement item)
        {
            this.item = item;
        }

        public string SelectValueSql
        {
            get
            {
                string s = string.Format("[Content].value('(/x:{0}/x:{1})[1]', '{2}') AS [{1}]", 
                    this.item.Parent.PrimaryElementName, 
                    this.item.Name, 
                    this.GetSqlType(this.item));
                return s;
            }
        }

        public string SelectQuerySql
        {
            get
            {
                string s = string.Format("[Content].query('(/x:{0}/x:{1})') AS [{1}]", 
                    this.item.Parent.PrimaryElementName,
                    this.item.Name);
                return s;
            }
        }

        private string GetSqlType(SchemaElement p)
        {
            switch (p.Type)
            {
                case "xs:string":
                    int maxLen = p.MaxLenth ?? 255;
                    return string.Format("varchar({0})", maxLen <= 8000 ? maxLen : 8000);
                case "booleanInt":
                    return "bit";
                case "xs:unsignedInt":
                    return "int";
                case "xs:short":
                    return "int";
                case "xs:decimal":
                    return "money";
                case "xs:anyURI":
                    return "varchar(255)";
                default:
                    return p.Type;
            }
        }
    }
}
