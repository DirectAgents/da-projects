using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAGenerators.Spreadsheets
{
    public partial class SearchReportPPC
    {
        public void Setup(string templateName)
        {
            bool useDefaultTemplate = false;
            switch (templateName)
            {
                case "ScholasticTeacherExpress":
                    Metric_Revenue = new Metric(3, "Revenue");
                    Metric_Cost = new Metric(4, "Cost");
                    Metric_ROAS = new Metric(5, "ROAS");
                    Metric_Orders = new Metric(6, "Orders");
                    Metric_Impressions = new Metric(7, "Impressions");
                    Metric_Clicks = new Metric(8, "Clicks");
                    Metric_CTR = new Metric(9, "CTR");
                    Metric_OrderRate = new Metric(10, "Order Rate");
                    Metric_CPC = new Metric(11, "CPC");
                    Metric_CPO = new Metric(12, "CPO");
                    break;
                default:
                    useDefaultTemplate = true;
                    Metric_Clicks = new Metric(3, "Clicks");
                    Metric_Impressions = new Metric(4, "Impressions");
                    Metric_Orders = new Metric(5, "Orders");
                    Metric_Cost = new Metric(7, "Cost");
                    Metric_Revenue = new Metric(8, "Revenue");

                    Metric_OrderRate = new Metric(6, "Order Rate");
                    Metric_Net = new Metric(9, "Net");
                    Metric_RevPerOrder = new Metric(10, "Revenue/Order");
                    Metric_CTR = new Metric(11, "CTR");
                    Metric_CPC = new Metric(12, "CPC");
                    Metric_CPO = new Metric(13, "CPO");
                    Metric_ROAS = new Metric(14, "ROAS");
                    Metric_ROI = new Metric(15, "ROI");
                    break;
            }
            if (!useDefaultTemplate)
                this.TemplateFilename = "SearchTemplate_" + templateName + ".xlsx";
        }
    }
}
