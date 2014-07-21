using ClientPortal.Web.Models;
using System.Collections.Generic;

namespace ClientPortal.Web.Areas.TD.Models
{
    public class TDReportModel
    {
        public TDReportModel(UserInfo userInfo, string metricToGraph1 = null, string metricToGraph2 = null)
        {
            UserInfo = userInfo;
            SetupMetrics();
            SetMetricsToGraph(metricToGraph1, metricToGraph2);
        }

        public void SetupMetrics()
        {
            List<string> metricsList = new List<string>();

            metricsList.Add("Impressions");
            metricsList.Add("Clicks");
            if (UserInfo.ShowConversionData)
                metricsList.Add("Conversions");
            metricsList.Add("Spend");

            AllMetrics = metricsList.ToArray();
        }

        public void SetMetricsToGraph(string metricToGraph1, string metricToGraph2)
        {
            List<string> metricsToGraphList = new List<string>();

            if (metricToGraph1 == null && AllMetrics.Length > 0)
                metricToGraph1 = AllMetrics[0];
            if (metricToGraph2 == null && AllMetrics.Length > 1)
                metricToGraph2 = AllMetrics[1];

            if (metricToGraph1 != null || metricToGraph2 != null)
                metricsToGraphList.Add(metricToGraph1);
            if (metricToGraph2 != null)
                metricsToGraphList.Add(metricToGraph2);

            MetricsToGraph = metricsToGraphList.ToArray();
        }

        private UserInfo UserInfo { get; set; }

        public string[] AllMetrics { get; set; }
        public string[] MetricsToGraph { get; set; }
    }
}