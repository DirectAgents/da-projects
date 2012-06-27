using System;
using System.Collections;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace DAgents.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicXML : DynamicObject, IEnumerable
    {
        private XElement Elm { get; set; }

        /// <summary>
        /// Create from a string containing XML content.
        /// </summary>
        /// <param name="xml"></param>
        public DynamicXML(string xml) { Elm = XElement.Parse(xml); }

        /// <summary>
        /// Create from an XElement.
        /// </summary>
        /// <param name="elm"></param>
        public DynamicXML(XElement elm) { Elm = elm; }

        /// <summary>
        /// Create from an XDocument.
        /// </summary>
        /// <param name="doc"></param>
        public DynamicXML(XDocument doc) { Elm = doc.Root; }

        /// <summary>
        /// Create from a file
        /// </summary>
        /// <param name="xml"></param>
        public DynamicXML(FileInfo file) { Elm = XElement.Load(File.ReadAllText(file.FullName)); }

        #region Operators

        /// <summary>
        /// Convert to String to get the text value of the element.
        /// </summary>
        /// <param name="dx"></param>
        /// <returns></returns>
        public static implicit operator string(DynamicXML dx) { return dx.Elm.Value; }

        /// <summary>
        /// Convert to DateTime to parse the text value of the element as a DateTime.
        /// </summary>
        /// <param name="dx"></param>
        /// <returns></returns>
        public static implicit operator DateTime(DynamicXML dx) { return DateTime.Parse(dx.Elm.Value); }

        /// <summary>
        /// Convert to Decimal to parse the text value of the element as a Decimal.
        /// </summary>
        /// <param name="dx"></param>
        /// <returns></returns>
        public static implicit operator decimal(DynamicXML dx) { return Decimal.Parse(dx.Elm.Value); }

        /// <summary>
        /// Convert to Boolean to parse the text value of the element as a Boolean.
        /// </summary>
        /// <param name="dx"></param>
        /// <returns></returns>
        public static implicit operator bool(DynamicXML dx) { return Boolean.Parse(dx.Elm.Value); }


        /// <summary>
        /// Convert to Int32 to parse the text value of the element as a Int32.
        /// </summary>
        /// <param name="dx"></param>
        /// <returns></returns>
        public static implicit operator int(DynamicXML dx) { return Int32.Parse(dx.Elm.Value); } 


        #endregion

        #region IEnumerable implementation

        /// <summary>
        /// Enumerate to get the children elements.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return Elm.Elements().Select(c => new DynamicXML(c)).GetEnumerator();
        }
 
        #endregion

        #region DynamicObject overrides

        /// <summary>
        /// The member named Count returns the number of child elements.
        /// All others return the element with the same name.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name == "Count")
            {
                result = Elm.Elements().Count();
            }
            else
            {
                var elms = Elm.Elements(binder.Name);

                if (elms.Count() > 0)
                    result = new DynamicXML(elms.First());
                else
                    result = null;
            }

            return true;
        }

        /// <summary>
        /// Getting a numerical index returns the element at the supplied zero-based location.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="indexes"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            int index = (int)indexes[0];

            result = new DynamicXML(Elm.Elements().ElementAt(index));

            return true;
        } 

        #endregion
    }
}
