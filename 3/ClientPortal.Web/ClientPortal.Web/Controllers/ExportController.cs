using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using CsvHelper;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class ExportController : Controller
    {
        private ICakeRepository cakeRepo;
        private IClientPortalRepository cpRepo;

        private static int MaxExportedRows = 1000;

        public ExportController(ICakeRepository cakeRepository, IClientPortalRepository cpRepository)
        {
            this.cakeRepo = cakeRepository;
            this.cpRepo = cpRepository;
        }

        public FileResult OfferSummaryReport(string startdate, string enddate)
        {
            var userProfile = HomeController.GetUserProfile();
            DateTime? start, end;

            if (!ControllerHelpers.ParseDates(startdate, enddate, userProfile.CultureInfo, out start, out end))
                return File("Error parsing dates: " + startdate + " and " + enddate, "text/plain");

            if (!start.HasValue)
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Get data and map to rows
            var records = cakeRepo.GetOfferInfos(start, end, userProfile.CakeAdvertiserId)
                .OrderByDescending(r => r.Revenue).ThenByDescending(r => r.Clicks).ThenBy(r => r.OfferId).Take(MaxExportedRows);
            var rows = Mapper.Map<IEnumerable<OfferInfo>, IEnumerable<OfferSummaryReportExportRow>>(records);
            return CsvFile(rows, "OfferSummary.csv");
        }

        public FileResult DailySummaryReport(string startdate, string enddate)
        {
            var userProfile = HomeController.GetUserProfile();
            DateTime? start, end;

            if (!ControllerHelpers.ParseDates(startdate, enddate, userProfile.CultureInfo, out start, out end))
                return File("Error parsing dates: " + startdate + " and " + enddate, "text/plain");

            if (!start.HasValue)
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Get data and map to rows
            var records = cakeRepo.GetDailyInfos(start, end, userProfile.CakeAdvertiserId)
                .OrderBy(r => r.Date).Take(MaxExportedRows);
            var rows = Mapper.Map<IEnumerable<DailyInfo>, IEnumerable<DailySummaryReportExportRow>>(records);
            return CsvFile(rows, "DailySummary.csv");
        }

        public FileResult ConversionReport(string startdate, string enddate)
        {
            var userProfile = HomeController.GetUserProfile();
            DateTime? start, end;

            if (!ControllerHelpers.ParseDates(startdate, enddate, userProfile.CultureInfo, out start, out end))
                return File("Error parsing dates: " + startdate + " and " + enddate, "text/plain");

            if (!start.HasValue)
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Get data and map to rows
            var records = cpRepo.GetConversionInfos(start, end, userProfile.CakeAdvertiserId, null)
                .OrderBy(r => r.Date).Take(MaxExportedRows);
            var rows = Mapper.Map<IEnumerable<ConversionInfo>, IEnumerable<ConversionReportExportRow>>(records);
            return CsvFile(rows, "Conversions.csv");
        }

        public FileResult AffiliateReport(string startdate, string enddate)
        {
            var userProfile = HomeController.GetUserProfile();
            DateTime? start, end;

            if (!ControllerHelpers.ParseDates(startdate, enddate, userProfile.CultureInfo, out start, out end))
                return File("Error parsing dates: " + startdate + " and " + enddate, "text/plain");

            if (!start.HasValue)
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Get data and map to rows
            var records = cakeRepo.GetAffiliateSummaries(start, end, userProfile.CakeAdvertiserId, null)
                .OrderBy(r => r.AffId).ThenBy(r => r.OfferId).Take(MaxExportedRows);
            var rows = Mapper.Map<IEnumerable<AffiliateSummary>, IEnumerable<AffiliateReportExportRow>>(records);
            return CsvFile(rows, "Affiliates.csv");
        }

        // ------- helpers ---------

        private FileResult CsvFile<T>(IEnumerable<T> rows, string downloadFileName)
            where T : class
        {
            var output = new MemoryStream();
            var writer = new StreamWriter(output);
            var csv = new CsvWriter(writer);
            csv.WriteRecords<T>(rows);
            writer.Flush();
            output.Position = 0;
            return File(output, "application/CSV", downloadFileName);
        }
    }
}
