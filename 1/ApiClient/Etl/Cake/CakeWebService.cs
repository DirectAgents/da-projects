﻿using System.Xml.Serialization;
using ApiClient.Models.Cake;
using Common;
using ApiClient.Models.Cake.Offers;
using System.IO;
using System;

namespace ApiClient.Etl.Cake
{
    public class CakeWebService
    {
        static string ApiKey = "FCjdYAcwQE";
        static string ConversionsUrl = @"http://login.directagents.com/api/4/reports.asmx/Conversions?api_key={api_key}&start_date={start_date}&end_date={end_date}&affiliate_id=0&advertiser_id=0&offer_id=0&campaign_id=0&creative_id=0&include_tests=false&start_at_row={start_at_row}&row_limit={row_limit}&sort_field=conversion_id&sort_descending=false";
        static string DailySummaryUrl = @"http://login.directagents.com/api/1/reports.asmx/DailySummaryExport?api_key={api_key}&start_date={start_date}&end_date={end_date}&affiliate_id=0&advertiser_id=0&offer_id={offer_id}&vertical_id=0&campaign_id=0&creative_id=0&account_manager_id=0&include_tests=false";
        static string OffersUrl = @"https://login.directagents.com/api/3/export.asmx/Offers?api_key={api_key}&offer_id=0&offer_name=&advertiser_id=0&vertical_id=0&offer_type_id=0&media_type_id=0&offer_status_id=0&tag_id=0&start_at_row=0&row_limit=0&sort_field=offer_name&sort_descending=false";
        
        public static conversion_report_response Conversions(DateRange dateRange, int startAtRow, int rowLimit)
        {
            Logger.Log("Extracting {0} conversions starting at row {1}.", rowLimit, startAtRow);
            string url = ConversionsUrl
                .Replace("{api_key}", ApiKey)
                .Replace("{start_date}", dateRange.FromDate.ToString("MM/dd/yyyy"))
                .Replace("{end_date}", dateRange.ToDate.ToString("MM/dd/yyyy"))
                .Replace("{row_limit}", rowLimit.ToString())
                .Replace("{start_at_row}", startAtRow.ToString());
            Logger.Log("HttpGet {0}", url);
            string content = Http.Get(url);
            return conversion_report_response.DeserializeFrom(Clean(content));
        }

        public static offer_export_response Offers()
        {
            string url = OffersUrl.Replace("{api_key}", ApiKey);
            Logger.Log("HttpGet {0}", url);
            string content = Http.Get(url);
            var serializer = new XmlSerializer(typeof(offer_export_response));
            var response = (offer_export_response)serializer.Deserialize(new StringReader(content));
            return response;
        }

        public static DailySummary[] DailySummaries(int offerId, DateRange dateRange)
        {
            string url = DailySummaryUrl
                .Replace("{api_key}", ApiKey)
                .Replace("{offer_id}", offerId.ToString())
                .Replace("{start_date}", dateRange.FromDate.ToString("MM/dd/yyyy"))
                .Replace("{end_date}", dateRange.ToDate.ToString("MM/dd/yyyy"));
            string content = Http.Get(url);
            var xmlSerializer = new XmlSerializer(typeof(ArrayOfDailySummary));
            DailySummary[] dailySummaries;
            var response = (ArrayOfDailySummary)xmlSerializer.Deserialize(new System.IO.StringReader(content));
            dailySummaries = response.DailySummary;
            if (dailySummaries == null)
            {
                dailySummaries = new DailySummary[] { };
            }
            foreach (var dailySummary in dailySummaries)
            {
                dailySummary.offer_id = offerId;
            }
            return dailySummaries;
        }

        private static string Clean(string content)
        {
            string cleaned = content
                                .Replace(@"<click_id xsi:nil=""true"" />", @"<click_id>0</click_id>")
                                .Replace(@"&#x0;", @"")
                                ;
            return cleaned;
        }
    }
}
