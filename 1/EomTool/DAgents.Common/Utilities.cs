using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
namespace DAgents.Common
{
    public static class Utilities
    {
        public static string MakeLegalFilename(string fileName)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }

            return fileName;
        }

        public static string FormatXml(XmlDocument xdoc)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var xw = new XmlTextWriter(sw);
            xw.Formatting = Formatting.Indented;
            xdoc.Save(xw);
            return sb.ToString();
        }

        public class XSettings : DynamicObject, IEnumerable
        {
            private XElement _xe;

            public XSettings(string xml)
            {
                this._xe = XElement.Parse(xml);
            }

            public XSettings(XElement xe)
            {
                this._xe = xe;
            }

            public XSettings(XDocument xd)
            {
                this._xe = xd.Root;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                string name = binder.Name;

                if (name == "Count")
                {
                    result = _xe.Elements().Count();
                    return true;
                }

                var q = _xe.Elements(name);

                if (q.Count() > 0)
                    result = new XSettings(q.First());
                else
                    result = null;

                return true;
            }

            public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
            {
                int index = (int)indexes[0];
                result = new XSettings(_xe.Elements().ElementAt(index));
                return true;
            }

            public static implicit operator string(XSettings x)
            {
                return x._xe.Value;
            }

            public static implicit operator DateTime(XSettings x)
            {
                return DateTime.Parse(x._xe.Value);
            }

            public static implicit operator decimal(XSettings x)
            {
                return Decimal.Parse(x._xe.Value);
            }

            public IEnumerator GetEnumerator()
            {
                return _xe.Elements().Select(c => new XSettings(c)).GetEnumerator();
            }
        }

        public static IList<dynamic> GetExpandoFromXml(XDocument doc, string elementName)
        {
            var list = new List<dynamic>();

            var nodes = from node in doc.Root.Descendants(elementName)
                        select node;

            foreach (var node in nodes)
            {
                dynamic expando = new ExpandoObject();

                foreach (var child in node.Descendants())
                {
                    (expando as IDictionary<String, object>)[child.Name.ToString()] = child.Value.Trim();
                }

                list.Add(expando);
            }

            return list;
        }

        public class ConsoleLogger : ILogger
        {
            public void Log(string message)
            {
                Console.WriteLine(message);
            }

            public void LogError(string message)
            {
                Console.WriteLine("Error! " + message);
            }
        }

        public class NullLogger : ILogger
        {
            public void Log(string message)
            {
            }

            public void LogError(string message)
            {
            }
        }

        public static class CopyUtility
        {
            public static void Copy(object sourceObject, object targetObject, bool deepCopy = true)
            {
                if (sourceObject != null && targetObject != null)
                {
                    (from sourceProperty in sourceObject.GetType().GetProperties().AsEnumerable()
                     from targetProperty in targetObject.GetType().GetProperties().AsEnumerable()
                     where sourceProperty.Name == targetProperty.Name
                     let sourceValue = sourceProperty.GetValue(sourceObject, null)
                     let targetValue = targetProperty.GetValue(targetObject, null)
                     where sourceValue != null && !sourceValue.Equals(targetValue)
                     select Action(targetProperty, targetObject, sourceValue, deepCopy))
                    .ToList()
                    .ForEach(c => c());
                }
            }

            static Action Action(PropertyInfo propertyInfo, object targetObject, object sourceValue, bool deepCopy)
            {
                Action action;
                if (sourceValue == null)
                    action = () => { };
                else if (!deepCopy || sourceValue.GetType().FullName.StartsWith("System."))
                    action = () => propertyInfo.SetValue(targetObject, sourceValue, null);
                else
                    action = () => Copy(sourceValue, propertyInfo.GetValue(targetObject, null));
                return action;
            }
        }
    }
}
