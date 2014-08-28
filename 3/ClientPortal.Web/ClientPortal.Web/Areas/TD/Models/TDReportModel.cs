using ClientPortal.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.TD.Models
{
    public class TDReportModel
    {
        private UserInfo UserInfo { get; set; }

        public SelectListItem[] AllMetricsSelectItems { get; set; }
        private string[] AllMetrics { get; set; }
        public string[] MetricsToGraph { get; set; } // e.g. the two metrics to graph on the summary page

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public TDReportModel(UserInfo userInfo, string metricToGraph1 = null, string metricToGraph2 = null)
        {
            UserInfo = userInfo;
            SetupMetrics();
            SetMetricsToGraph(metricToGraph1, metricToGraph2);

            StartDate = userInfo.Dates.FirstOfMonth.ToString("d", userInfo.CultureInfo);
            EndDate = userInfo.Dates.Yesterday.ToString("d", userInfo.CultureInfo);
        }

        public void SetupMetrics()
        {
            var metricsItemsList = new List<SelectListItem>();

            metricsItemsList.Add(new SelectListItem { Text = "Impressions", Value = "Impressions" });
            metricsItemsList.Add(new SelectListItem { Text = "Clicks", Value = "Clicks" });
            if (UserInfo.TDAccount.ShowConversions)
                metricsItemsList.Add(new SelectListItem { Text = "Conversions", Value = "Conversions" });
            metricsItemsList.Add(new SelectListItem { Text = "Media Spend", Value = "Spend" });

            metricsItemsList.Add(new SelectListItem { Text = "eCPM", Value = "CPM" });
            metricsItemsList.Add(new SelectListItem { Text = "eCPC", Value = "CPC" });
            if (UserInfo.TDAccount.ShowConversions)
                metricsItemsList.Add(new SelectListItem { Text = "eCPA", Value = "CPA" });

            metricsItemsList.Add(new SelectListItem { Text = "CTR", Value = "CTR" });
            if (UserInfo.TDAccount.ShowConversions)
                metricsItemsList.Add(new SelectListItem { Text = "ConvRate", Value = "ConvRate" });

            AllMetricsSelectItems = metricsItemsList.ToArray();
            AllMetrics = metricsItemsList.Select(m => m.Value).ToArray();
        }

        public void SetMetricsToGraph(string metricToGraph1, string metricToGraph2)
        {
            List<string> metricsToGraphList = new List<string>();

            if (metricToGraph1 == null && metricToGraph2 == null)
            {
                if (AllMetrics.Length > 0)
                    metricToGraph1 = AllMetrics[0];
                if (AllMetrics.Length > 1)
                    metricToGraph2 = AllMetrics[1];
            }
            if (metricToGraph1 != null || metricToGraph2 != null)
                metricsToGraphList.Add(metricToGraph1);
            if (metricToGraph2 != null && metricToGraph2 != metricToGraph1)
                metricsToGraphList.Add(metricToGraph2);

            MetricsToGraph = metricsToGraphList.ToArray();
        }

        public bool ShouldShowMetric(string metric)
        {
            return AllMetrics.Contains(metric);
        }
    }
}