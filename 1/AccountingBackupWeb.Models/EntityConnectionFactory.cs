using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data.SqlClient;

namespace AccountingBackupWeb.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityConnectionFactory<T>
    {
        public EntityConnectionFactory(string sqlServerName, string databaseName, string metaData)
            : this(sqlServerName, databaseName, metaData, "System.Data.SqlClient")
        {
        }

        public EntityConnectionFactory(string sqlServerName, string databaseName, string metaData, string dataProviderName)
        {
            SqlServerName = sqlServerName;
            DatabaseName = databaseName;
            Metadata = metaData;
            DataProviderName = dataProviderName;
        }

        public T Create()
        {
            return (T)Activator.CreateInstance(typeof(T), CreateEntityConnection());
        }

        EntityConnection CreateEntityConnection()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = SqlServerName;
            sqlBuilder.InitialCatalog = DatabaseName;
            sqlBuilder.IntegratedSecurity = true;

            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.Provider = DataProviderName;
            entityBuilder.ProviderConnectionString = sqlBuilder.ToString();
            entityBuilder.Metadata = Metadata;

            return new EntityConnection(entityBuilder.ConnectionString);
        }

        public string DatabaseName { get; set; }

        public string SqlServerName { get; set; }

        public string DataProviderName { get; set; }

        private string metaData;
        public string Metadata
        {
            get
            {
                string result;
                if (!this.metaData.StartsWith("res://"))
                {
                    result = string.Format(@"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", this.metaData);
                }
                else
                {
                    result = this.metaData;
                }
                return result;
            }
            set
            {
                this.metaData = value;
            }
        }
    }
}
