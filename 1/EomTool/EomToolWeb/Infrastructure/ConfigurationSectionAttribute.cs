using System;

namespace EomToolWeb.Infrastructure
{
    public class ConfigurationSectionAttribute : Attribute
    {
        public ConfigurationSectionAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}