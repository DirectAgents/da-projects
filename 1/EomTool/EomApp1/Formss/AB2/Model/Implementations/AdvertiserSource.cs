using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Formss.AB2.Model
{
    public class AdvertiserSource : IAdvertiserSource
    {
        private readonly string _dbName;
        private readonly DirectAgentsEntities _model;

        private IEnumerable<SqlServerDatabase> ExternalDatabases
        {
            get
            {
                return string.IsNullOrEmpty(_dbName)
                                ? _model.SqlServerDatabases
                                : _model.SqlServerDatabases.Where(c => c.Name == _dbName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sqlServerDatabaseName">null/empty --> all databases</param>
        public AdvertiserSource(string sqlServerDatabaseName, DirectAgentsEntities model)
        {
            _dbName = sqlServerDatabaseName;
            _model = model;
        }

        public IEnumerable<Advertiser> Advertisers
        {
            get
            {
                var advertiserSet = new HashSet<string>();
                foreach (var externalDatabase in ExternalDatabases)
                {
                    using (var externalDatabaseModel = ExternalDatabase.ExternalDatabaseModel.CreateExternalDatabaseModel(externalDatabase.ConnectionString))
                    {
                        foreach (var externalAdvertiser in externalDatabaseModel.Advertisers)
                        {
                            if (!advertiserSet.Contains(externalAdvertiser.name))
                            {
                                string clientCurrencyName = externalAdvertiser.Currency != null
                                    ? externalAdvertiser.Currency.name
                                    : "USD";

                                Unit clientCurrency = _model.Units.First(c => c.Name == clientCurrencyName);

                                PayTerm clientPayTerm = _model.PayTerms.First(c => c.Name == "Net 30");

                                Client client = new Client {
                                    Name = externalAdvertiser.name,
                                    PayTerm = clientPayTerm,
                                    CreditLimit = new CreditLimit(0, clientCurrency)
                                };

                                Advertiser advertiser = new Advertiser(externalAdvertiser.name, client);

                                advertiserSet.Add(externalAdvertiser.name);

                                yield return advertiser;
                            }
                        }
                    }
                }
            }
        }
    }
}
