using CakeExtracter.Helpers;
using CakeExtractor.SeleniumApplication.Loaders.VCD.Constants;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Loaders.VCD
{
    internal class AmazonVcdLoader
    {
        public void PrepareLoader()
        {
            EnsureMetricTypes();
        }

        public void LoadDailyVendorCentralData(VcdReportData reportData, DateTime date)
        {
        }

        private void EnsureMetricTypes()
        {
            SafeContextWrapper.SaveChangedContext<ClientPortalProgContext>(
            SafeContextWrapper.MetricTypeLocker, db =>
            {
                VendorCentralDataLoadingConstants.VendorMetricTypeNames.ForEach(metricType =>
                {
                    var existingMetricType = db.MetricTypes.FirstOrDefault(mt => mt.Name == metricType);
                    if (existingMetricType == null)
                    {
                        db.MetricTypes.Add(new MetricType(metricType, VendorCentralDataLoadingConstants.VendorMetricsDaysInterval));
                    }
                });
            });
        }
    }
}
