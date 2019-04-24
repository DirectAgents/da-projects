using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.DBM.Extractors.Composer;
using CakeExtracter.Etl.DBM.Extractors.Parser;
using CakeExtracter.Etl.DBM.Extractors.Parser.ParsingConverters;
using CakeExtracter.Etl.DBM.Models;
using DBM;
using DBM.Parser.Models;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;

namespace CakeExtracter.Etl.DBM.Extractors
{
    internal class DbmCreativeExtractor: DbmApiExtractor<DbmCreativeSummary>
    {
        private readonly DbmReportDataComposer composer;
        private readonly DbmAccountReportData reportData;

        public DbmCreativeExtractor(DbmAccountReportData reportData) 
           // : base(dbmUtility, dateRange)
        {
            composer = new DbmReportDataComposer();
            this.reportData = reportData;
        }

        protected override void Extract()
        {
            try
            {
                var creativeSummaries = composer.ComposeCreativeReportData(reportData);
                //var reportCreativeRows = parser.EnumerateRows();
                //var reportCreativeRowsGroupedByAccounts = composer.ComposeReportData(reportCreativeRows, accounts);

                //var creativeSummaries = new List<DbmCreativeSummary>();
                //foreach (var group in reportCreativeRowsGroupedByAccounts)
                //{
                //    var summaries = CreateCreativeSummary(group);
                //    creativeSummaries.AddRange(summaries);
                //}
               
                Add(creativeSummaries);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            End();
        }

        
    }
}
