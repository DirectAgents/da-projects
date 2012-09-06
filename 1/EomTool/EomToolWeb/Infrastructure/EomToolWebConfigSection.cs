using System;
using System.Configuration;

namespace EomToolWeb.Infrastructure
{
    [ConfigurationSection("eomToolWeb")]
    public class EomToolWebConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("replaceIntegratedSecurityWithSALogin", DefaultValue = "false", IsRequired = false)]
        public Boolean ReplaceIntegratedSecurityWithSALogin
        {
            get { return (Boolean)this["replaceIntegratedSecurityWithSALogin"]; }
            set { this["replaceIntegratedSecurityWithSALogin"] = value; }
        }

        [ConfigurationProperty("saPassword", DefaultValue = "", IsRequired = false)]
        public String SAPassword
        {
            get { return (string)this["saPassword"]; }
            set { this["saPassword"] = value; }
        }

        [ConfigurationProperty("debugMode", DefaultValue = "false", IsRequired = false)]
        public Boolean DebugMode
        {
            get { return (Boolean)this["debugMode"]; }
            set { this["debugMode"] = value; }
        }

        public string GetConnectionString(string connectionStringName)
        {
            ConnectionStringSettings connectionString;
            if (this.CurrentConfiguration.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                connectionString = this.CurrentConfiguration.ConnectionStrings.ConnectionStrings[connectionStringName];
                if (connectionString != null)
                    return connectionString.ConnectionString;
            }
            throw new Exception("Failed to obtain connection string for " + connectionStringName);
        }

        public string MasterConnectionString
        {
            get { return GetConnectionString("DAMain1"); }
        }
    }
}