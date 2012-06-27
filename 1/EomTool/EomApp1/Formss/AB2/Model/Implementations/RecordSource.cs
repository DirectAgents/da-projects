using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EomApp1.Formss.AB2.Model.Interfaces;

namespace EomApp1.Formss.AB2.Model
{
    public class RecordSource : IRecordSource
    {
        public RecordSource(string externalDatabaseName, DirectAgentsEntities model)
        {
            _externalDatabaseName = externalDatabaseName;
            _model = model;
        }

        private readonly string _externalDatabaseName;
        private readonly DirectAgentsEntities _model;

        public IEnumerable<SqlServerDatabase> SqlServerDatabases
        {
            get
            {
                var a = new SqlServerDatabaseSource(@"C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\AB2\Model\Files\ModelSettings.xml"); // todo: DI
                return a.SqlServerDatabases;
            }
        }

        public IEnumerable<string> UnitNames
        {
            get
            {
                var a = new UnitNameSource();
                return a.UnitNames;
            }
        }

        public IEnumerable<UnitConversion> UnitConversions
        {
            get
            {
                var a = new UnitConversionSource(_externalDatabaseName);
                return a.UnitConversions;
            }
        }

        public IEnumerable<Advertiser> Advertisers
        {
            get
            {
                var a = new AdvertiserSource(_externalDatabaseName, _model);
                return a.Advertisers;
            }
        }

        public IEnumerable<PayTerm> PayTerms
        {
            get 
            {
                var a = new PayTermSource();
                return a.PayTerms;
            }
        }

        public IEnumerable<StartingBalance> StartingBalances
        {
            get
            {
                var a = new StartingBalanceSource(_model);
                return a.StartingBalances;
            }
        }
    }
}
