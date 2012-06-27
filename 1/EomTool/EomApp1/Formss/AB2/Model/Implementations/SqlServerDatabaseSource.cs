using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Practices.Unity;

namespace EomApp1.Formss.AB2.Model
{
    public class SqlServerDatabaseSource : ISqlServerDatabaseSource
    {       
        private readonly string _settingsFile;
        
        public SqlServerDatabaseSource(string settingsFile)
        {
            _settingsFile = settingsFile;
        }

        public IEnumerable<SqlServerDatabase> SqlServerDatabases
        {
            get
            {
                XDocument settingsXDocument = XDocument.Load(_settingsFile);

                IEnumerable<XElement> directAgentsDatabaseXElements = settingsXDocument
                    .Element("Settings")
                        .Element("DirectAgentsDatabases")
                            .Elements("DirectAgentsDatabase");

                foreach (XElement directAgentsDatabaseXElement in directAgentsDatabaseXElements)
                {
                    SqlServerDatabase sqlServerDatabase = new SqlServerDatabase {
                        Name = directAgentsDatabaseXElement.Element("DatabaseName").Value,
                        ConnectionString = directAgentsDatabaseXElement.Element("ConStr").Value,
                        Period = new Period {
                            Name = directAgentsDatabaseXElement.Element("PeriodName").Value,
                            DateSpan = new DateSpan {
                                From = DateTime.Parse(directAgentsDatabaseXElement.Element("FromDate").Value),
                                To = DateTime.Parse(directAgentsDatabaseXElement.Element("ToDate").Value)
                            }
                        }
                    };

                    yield return sqlServerDatabase;
                }
            }
        }

        [Dependency("SettingsFile")]
        public string SettingsFile { get; set; }
    }
}
