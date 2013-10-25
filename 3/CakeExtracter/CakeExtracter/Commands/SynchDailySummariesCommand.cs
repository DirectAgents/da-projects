﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchDailySummariesCommand : ConsoleCommand
    {
        public string Advertiser { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            Advertiser = null;
            StartDate = null;
            EndDate = null;
        }

        public SynchDailySummariesCommand()
        {
            RunBefore(new SynchAdvertisersCommand());
            RunBefore(new SynchOffersCommand());

            IsCommand("synchDailySummaries", "synch DailySummaries for an advertisers offers in a date range");
            HasOption("a|advertiserId=", "Advertiser Id (* = all advertisers)", c => Advertiser = c);
            HasOption("s|startDate=", "Start Date (default is two months ago)", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is today)", c => EndDate = DateTime.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var twoMonthAgo = DateTime.Today.AddMonths(-2);
            var dateRange = new DateRange(StartDate ?? twoMonthAgo, EndDate ?? DateTime.Today);

            dateRange.ToDate = dateRange.ToDate.AddDays(1); // cake requires the date _after_ the last date you want stats for

            foreach (var advertiserId in GetAdvertiserIds())
            {
                var extracter = new DailySummariesExtracter(dateRange, advertiserId);
                var loader = new DailySummariesLoader();
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
            }
            return 0;
        }

        private IEnumerable<int> GetAdvertiserIds()
        {
            if (string.IsNullOrWhiteSpace(Advertiser) || Advertiser == "*")
            {
                List<int> advertiserIds;
                using (var db = new ClientPortal.Data.Contexts.ClientPortalContext())
                {
                    advertiserIds = db.Advertisers
                                      .OrderBy(c => c.AdvertiserId)
                                      .Select(c => c.AdvertiserId)
                                      .ToList();
                }
                foreach (var advertiserId in advertiserIds)
                {
                    yield return advertiserId;
                }
            }
            else
            {
                yield return int.Parse(Advertiser);
            }
        }

    }
}