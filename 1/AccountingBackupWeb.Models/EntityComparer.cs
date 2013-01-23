using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AccountingBackupWeb.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityComparer<T> : IEqualityComparer<T>
    {
        string[] nameProperties = new[] { "name", "Name" };

        public bool Equals(T x, T y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) return false;
            var xValue = GetIdentyfyingPropertyValue(x);
            var yValue = GetIdentyfyingPropertyValue(y);
            var result = (xValue == yValue);
            return result;
        }

        public int GetHashCode(T x)
        {
            if (Object.ReferenceEquals(x, null)) return 0;
            return GetIdentyfyingPropertyValue(x).GetHashCode();
        }

        string GetIdentyfyingPropertyValue(T x)
        {
            return GetPropertyValue(x, GetIdentifyingPropertyName(x));
        }

        string GetIdentifyingPropertyName(T x)
        {
            string result = null;
            foreach (var nameProperty in this.nameProperties)
            {
                PropertyInfo propertyInfo = x.GetType().GetProperty(nameProperty);
                if (propertyInfo != null)
                {
                    result = nameProperty;
                }
            }
            if (result == null)
            {
                throw new Exception("no name property found");
            }
            return result;
        }

        string GetPropertyValue(T x, string nameProperty)
        {
            string result = x.GetType().GetProperty(nameProperty).GetValue(x, null) as string;
            if (result == null)
            {
                throw new Exception("property value must be a string");
            }
            return result;
        }
    }
}
