using System.Collections.Generic;
using System.IO;
using System.Linq;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class AmazonCsvExtractor<T> : Extracter<T>
    {
        private const char Delimiter = ',';
        private readonly string csvPath;

        protected AmazonCsvExtractor(string csvPath)
        {
            this.csvPath = csvPath;
        }

        protected List<Dictionary<string, string>> ExtractCsvRows()
        {
            using (var textReader = new StreamReader(csvPath))
            {
                var line = textReader.ReadLine();
                var headerLine = line.Split(Delimiter);
                var skipCount = 0;
                while (line != null && skipCount < 1)
                {
                    line = textReader.ReadLine();
                    skipCount++;
                }
                var campaignList = new List<Dictionary<string, string>>();
                while (line != null)
                {
                    var columns = line.Split(Delimiter);
                    var campaignItem = new Dictionary<string, string>();
                    for (var i = 0; i <= columns.Length - 1; i++)
                    {
                        campaignItem.Add(headerLine[i], columns[i]);
                    }
                    campaignList.Add(campaignItem);
                    line = textReader.ReadLine();
                }
                return campaignList;
            }
        }
    }

    public class AmazonCampaignCsvExtractor : AmazonCsvExtractor<StrategySummary>
    {
        public AmazonCampaignCsvExtractor(string csvPath) : base(csvPath)
        {
        }

        protected override void Extract()
        {
            var csvRows = ExtractCsvRows();
            var items = csvRows.Select(ConvertToModel);
            Add(items);
            End();
        }

        protected StrategySummary ConvertToModel(Dictionary<string, string> properties)
        {
            return new StrategySummary();
        }
    }
}
