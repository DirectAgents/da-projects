using System;

namespace LTWeb.Service.Tracking
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class UrlParamAttribute : Attribute
    {
        private readonly string paramName;

        public UrlParamAttribute(string paramName)
        {
            this.paramName = paramName;
        }

        public string ParamName
        {
            get { return this.paramName; }
        }
    }
}