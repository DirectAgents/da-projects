﻿using CsvHelper;
using CsvHelper.Configuration;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.IO;

namespace CakeExtracter.Etl.TradingDesk.Extracters.SummaryCsvExtracters
{
    public class YAMRow
    {
        public DateTime Day { get; set; }
        public string PixelParameter { get; set; }
        public int ClickThruConvs { get; set; }
        public int ViewThruConvs { get; set; }

        public string LineName { get; set; }
        public string LineID { get; set; }
    }

    public class YamConValCsvExtracter<T> : SummaryCsvExtracter<YAMRow>
        where T: CsvClassMap
    {
        public YamConValCsvExtracter(StreamReader streamReader = null, string csvFilePath = null)
            : base("YamConVals", null, streamReader, csvFilePath)
        {
        }

        protected override void SetupCsvReader(CsvReader csvReader)
        {
            base.SetupCsvReaderConfig(csvReader);
            csvReader.Configuration.RegisterClassMap<T>();
        }

        protected override void AddPropertiesMaps(CsvClassMap classMap, Type classType, ColumnMapping colMap)
        {
        }
    }

    public sealed class YAMRowMap : CsvClassMap<YAMRow>
    {
        public YAMRowMap()
        {
            Map(x => x.Day);
            Map(x => x.PixelParameter).Name("Pixel Query String");
            Map(x => x.ClickThruConvs).Name("Click Through Conversion");
            Map(x => x.ViewThruConvs).Name("View Through Conversion");
        }
    }

    public sealed class YAMRowMap_WithLine : CsvClassMap<YAMRow>
    {
        public YAMRowMap_WithLine()
        {
            Map(x => x.Day);
            Map(x => x.PixelParameter).Name("Pixel Query String");
            Map(x => x.ClickThruConvs).Name("Click Through Conversion");
            Map(x => x.ViewThruConvs).Name("View Through Conversion");
            Map(x => x.LineName).Name("Line");
            Map(x => x.LineID).Name("Line Id");
        }
    }
}
