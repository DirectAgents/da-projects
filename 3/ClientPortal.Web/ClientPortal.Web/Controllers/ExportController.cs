using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class ExportController : CPController
    {
        private static int MaxExportedRows = 1000;

        public ExportController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public FileResult OfferSummaryReport(string startdate, string enddate)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;

            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return File("Error parsing dates: " + startdate + " and " + enddate, "text/plain");

            if (!start.HasValue)
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Get data and map to rows
            var records = cpRepo.GetOfferInfos(start, end, userInfo.AdvertiserId)
                .OrderByDescending(r => r.Revenue).ThenByDescending(r => r.Clicks).ThenBy(r => r.OfferId).Take(MaxExportedRows);
            var rows = Mapper.Map<IEnumerable<OfferInfo>, IEnumerable<OfferSummaryReportExportRow>>(records);
            return CsvFile(rows, "OfferSummary" + ControllerHelpers.DateStamp() + ".csv");
        }

        public FileResult DailySummaryReport(string startdate, string enddate, int? offerid)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;

            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return File("Error parsing dates: " + startdate + " and " + enddate, "text/plain");

            if (!start.HasValue)
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Get data and map to rows
            var records = cpRepo.GetDailyInfos(start, end, userInfo.AdvertiserId, offerid)
                .OrderBy(r => r.Date).Take(MaxExportedRows);
            var rows = Mapper.Map<IEnumerable<DailyInfo>, IEnumerable<DailySummaryReportExportRow>>(records);
            return CsvFile(rows, "DailySummary" + ControllerHelpers.DateStamp() + ".csv");
        }

        public FileResult ConversionReport(string startdate, string enddate, int? offerid)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;

            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return File("Error parsing dates: " + startdate + " and " + enddate, "text/plain");

            if (!start.HasValue)
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Get data and map to rows
            var records = cpRepo.GetConversionInfos(start, end, userInfo.AdvertiserId, offerid)
                .OrderBy(r => r.Date).Take(MaxExportedRows);
            var rows = Mapper.Map<IEnumerable<ConversionInfo>, IEnumerable<ConversionReportExportRow>>(records);
            return CsvFile(rows, "Conversions" + ControllerHelpers.DateStamp() + ".csv");
        }

        public FileResult AffiliateReport(string startdate, string enddate)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;

            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return File("Error parsing dates: " + startdate + " and " + enddate, "text/plain");

            if (!start.HasValue)
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Get data and map to rows
            var records = cpRepo.GetAffiliateSummaries(start, end, userInfo.AdvertiserId, null)
                .OrderBy(r => r.AffId).ThenBy(r => r.OfferId).Take(MaxExportedRows);
            var rows = Mapper.Map<IEnumerable<AffiliateSummary>, IEnumerable<AffiliateReportExportRow>>(records);
            return CsvFile(rows, "Affiliates" + ControllerHelpers.DateStamp() + ".csv");
        }

        public FileResult CPMSummaryReport(string startdate, string enddate)
        {
            var userInfo = GetUserInfo();
            DateTime? start, end;

            if (!ControllerHelpers.ParseDates(startdate, enddate, userInfo.CultureInfo, out start, out end))
                return File("Error parsing dates: " + startdate + " and " + enddate, "text/plain");

            if (!start.HasValue)
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Get data and map to rows
            var records = cpRepo.GetMonthlyInfos("CPM", start, end, userInfo.AdvertiserId)
                .OrderBy(r => r.Offer).ThenBy(r => r.Year).ThenBy(r => r.Month).Take(MaxExportedRows);
            var rows = Mapper.Map<IEnumerable<MonthlyInfo>, IEnumerable<CPMReportExportRow>>(records);
            return CsvFile(rows, "CPM" + ControllerHelpers.DateStamp() + ".csv");
        }

        // ------- helpers ---------

        private FileResult CsvFile<T>(IEnumerable<T> rows, string downloadFileName)
            where T : class
        {
            return File(ControllerHelpers.CsvStream(rows), "application/CSV", downloadFileName);
        }
    }
}
