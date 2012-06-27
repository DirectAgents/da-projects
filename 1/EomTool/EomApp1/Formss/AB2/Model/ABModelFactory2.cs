using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using DAgents.Common;
using Microsoft.Practices.Unity;

namespace EomApp1.Formss.AB2.Model
{
    public class ABModelFactory2 : IDirectAgentsEntitiesFactory
    {
        public void Init()
        {
            // Delete data
            using (var model = new DirectAgentsEntities(EntityConnectionString))
            {
                model.DeleteQuickBooksCustomers();
                model.DeleteQuickBooksCompanyFiles();
                model.DeleteUnitConversions();
                model.DeleteUnits();
                model.DeleteSqlServerDatabases();
                model.DeletePeriods();
                model.DeleteDateSpans();
                model.DeletePayTerms();
            }

            // Insert data
            InitUnits();
            InitUnitConversions();
            InitSqlServerDatabases();
            InitPayTerms();
            InitQuickBooksCompanyFiles();
        }

        public DirectAgentsEntities GetModel()
        {
            return new DirectAgentsEntities(this.EntityConnectionString);
        }

        private void InitQuickBooksCompanyFiles()
        {
            using (var model = new DirectAgentsEntities(EntityConnectionString))
            {
                model.QuickBooksCompanyFiles.AddObjects(QuickBooksCompanyFileSource.QuickBooksCompanyFiles.ToArray());
                model.SaveChanges();
            }
        }

        private void InitPayTerms()
        {
            using (var model = new DirectAgentsEntities(EntityConnectionString))
            {
                model.PayTerms.AddObjects(PayTermSource.PayTerms.ToArray());
                model.SaveChanges();
            }
        }

        private void InitSqlServerDatabases()
        {
            using (var model = new DirectAgentsEntities(EntityConnectionString))
            {
                model.SqlServerDatabases.AddObjects(SqlServerDatabaseSource.SqlServerDatabases.ToArray());
                model.SaveChanges();
            }
        }

        private void InitUnitConversions()
        {
            using (var model = new DirectAgentsEntities(EntityConnectionString))
            {
                //model.UnitConversions.AddObjects(UnitConversionSource.UnitConversions(model).ToArray());
                //model.SaveChanges();
            }
        }

        private void InitUnits()
        {
            using (var model = new DirectAgentsEntities(EntityConnectionString))
            {
                foreach (var item in UnitNameSource.UnitNames)
                    model.Units.AddObject(new Unit { Name = item });
                model.SaveChanges();
            }
        }

        [Dependency("EntityConnectionString")]
        public string EntityConnectionString { get; set; }

        [Dependency]
        public IUnitNameSource UnitNameSource { get; set; }

        [Dependency]
        public IUnitConversionSource UnitConversionSource { get; set; }

        [Dependency]
        public ISqlServerDatabaseSource SqlServerDatabaseSource { get; set; }

        [Dependency]
        public IPayTermSource PayTermSource { get; set; }

        [Dependency]
        public IQuickBooksCompanyFileSource QuickBooksCompanyFileSource { get; set; }
    }
}