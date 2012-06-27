using System.Collections.Generic;
using System.Linq;

namespace EomApp1.Formss.AB2.Model
{
    public class UnitConversionSource : EomApp1.Formss.AB2.Model.IUnitConversionSource
    {
        public UnitConversionSource(string sqlServerDatabaseName)
        {
            _dbName = sqlServerDatabaseName;
        }

        private readonly string _dbName;

        public IEnumerable<UnitConversion> UnitConversions
        {
            get
            {
                UnitConversion unitConversion;
                using (var model = new DirectAgentsEntities())
                {
                    foreach (var sqlServerDatabase in model.SqlServerDatabases)
                    {
                        using (var externalModel = ExternalDatabase.ExternalDatabaseModel.CreateExternalDatabaseModel(sqlServerDatabase.ConnectionString))
                        {
                            Dictionary<string, Unit> externalUnits = model.Units.ToDictionary(c => c.Name);
                            foreach (var externalCurrency in from c in externalModel.Currencies select new { Name = c.name, ToUSD = c.to_usd_multiplier })
                            {
                                unitConversion = new UnitConversion {
                                    From = model.Units.First(c => c.Name == externalCurrency.Name),
                                    To = model.Units.First(c => c.Name == "USD"),
                                    Multiplier = externalCurrency.ToUSD,
                                    DateSpan = sqlServerDatabase.Period.DateSpan
                                };
                                model.Detach(unitConversion);
                                yield return unitConversion;
                            }
                        }
                    }
                }
            }
        }
    }
}
