using CakeExtracter.Etl.DSP.Loaders.Constants;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.DSP.Loaders
{
    internal class AmazonDspDataLoader
    {
        private Dictionary<string, int> metricTypes;

        public AmazonDspDataLoader()
        {
            metricTypes = new Dictionary<string, int>();
            EnsureMetricTypes();
        }

        public void LoadData(List<AmazonDspAccauntReportData> accountsReportData)
        {
            try
            {
                accountsReportData.ForEach(rd => 
                {
                    LoadAccountData(rd);
                });
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadAccountData(AmazonDspAccauntReportData accountReportData)
        {

        }

        private void EnsureMetricTypes()
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                DspMetricConstants.MetricTypeNames.ForEach(metricTypeName =>
                {
                    var metricType = dbContext.MetricTypes.FirstOrDefault(mt => mt.Name == metricTypeName);
                    if (metricType == null)
                    {
                        metricType = new MetricType(metricTypeName, DspMetricConstants.VendorMetricsDaysInterval);
                        dbContext.MetricTypes.Add(metricType);
                        dbContext.SaveChanges();
                    }
                    metricTypes[metricTypeName] = metricType.Id;
                });
            }
        }
    }
}
