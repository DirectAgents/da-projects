using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Services;
using Google.Apis.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace CakeExtracter.Etl.SearchMarketing.Extracters
{
    public class AnalyticsApiExtracter : Extracter<AnalyticsRow>
    {
        private readonly string clientCustomerId;
        private readonly DateTime startDate;
        private readonly DateTime endDate;

        public AnalyticsApiExtracter(string clientCustomerId, CakeExtracter.Common.DateRange dateRange)
        {
            this.clientCustomerId = clientCustomerId;
            this.startDate = dateRange.FromDate;
            this.endDate = dateRange.ToDate;
        }

        protected override void Extract()
        {
            if (this.clientCustomerId == "334-498-7340") // SherrillTree
            {
                var rows = EnumerateAnalyticsRows("14958389");
                Add(rows);
            }
            End();
        }

        private IEnumerable<AnalyticsRow> EnumerateAnalyticsRows(string profileId)
        {
            string certPath = ConfigurationManager.AppSettings["GoogleAPI_Certificate"];
            var certificate = new X509Certificate2(certPath, "notasecret", X509KeyStorageFlags.Exportable);
            var provider = new AssertionFlowClient(GoogleAuthenticationServer.Description, certificate)
            {
                ServiceAccountId = ConfigurationManager.AppSettings["GoogleAPI_ServiceEmail"],
                Scope = AnalyticsService.Scopes.AnalyticsReadonly.GetStringValue()
            };
            var auth = new OAuth2Authenticator<AssertionFlowClient>(provider, AssertionFlowClient.GetState);
            var service = new AnalyticsService(new BaseClientService.Initializer()
            {
                Authenticator = auth,
                ApplicationName = "DA Client Portal"
            });
            string startDate = this.startDate.ToString("yyyy-MM-dd");
            string endDate = this.endDate.ToString("yyyy-MM-dd");
            string metrics = "ga:transactions,ga:transactionRevenue";
            DataResource.GaResource.GetRequest request = service.Data.Ga.Get("ga:" + profileId, startDate, endDate, metrics);
            request.Dimensions = "ga:date,ga:adwordsCampaignID";
            //request.MaxResults = 

            GaData gaData = request.Execute();
            // TODO: pagination

            foreach (var row in gaData.Rows)
            {
                AnalyticsRow aRow = null;
                try
                {
                    aRow = new AnalyticsRow()
                    {
                        Date = DateTime.ParseExact(row[0], "yyyyMMdd", CultureInfo.InvariantCulture),
                        CampaignId = int.Parse(row[1]),
                        Transactions = int.Parse(row[2]),
                        Revenue = decimal.Parse(row[3])
                    };
                }
                catch (Exception) { }

                // note: 'total' rows will be ignored because their CampaignId is "(not set)" and won't be int.Parsed

                if (aRow != null)
                    yield return aRow;
            }
        }

    }

    public class AnalyticsRow
    {
        public DateTime Date { get; set; }
        public int CampaignId { get; set; }
        public int Transactions { get; set; }
        public decimal Revenue { get; set; }
    }
}
