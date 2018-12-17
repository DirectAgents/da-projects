﻿using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters;
using CakeExtractor.SeleniumApplication.Models;
using CsvHelper.Configuration;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.AmazonReportCsvExtractors
{
    public class AmazonReportCsvExtractor : SummaryCsvExtracter<AmazonReportSummary>
    {
        private static AmazonReportColumnMapping columnMapping;

        static AmazonReportCsvExtractor()
        {
            InitializeColumnMapping();
        }

        public AmazonReportCsvExtractor(string csvFilePath)
            : base("Amazon report summaries", columnMapping, null, csvFilePath)
        {
        }

        private static void InitializeColumnMapping()
        {
            columnMapping =  new AmazonReportColumnMapping();
            columnMapping.SetDefaults();
        }

        protected override void AddPropertiesMaps(CsvClassMap classMap, Type classType, ColumnMapping colMap)
        {
            base.AddPropertiesMaps(classMap, classType, colMap);
            if (!(colMap is AmazonReportColumnMapping amazonColMapping))
            {
                return;
            }
            CheckAddPropertyMap(classMap, classType, "DetailPageViews", amazonColMapping.DetailPageViews);
            CheckAddPropertyMap(classMap, classType, "UnitsSold", amazonColMapping.UnitsSold);
            CheckAddPropertyMap(classMap, classType, "AttributedSales14D", amazonColMapping.AttributedSales14D);
        }
    }
}
