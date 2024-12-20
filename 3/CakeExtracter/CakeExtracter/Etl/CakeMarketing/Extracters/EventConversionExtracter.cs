﻿using System;
using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Exceptions;
using CakeExtracter.Common.JobExecutionManagement;

namespace CakeExtracter.Etl.CakeMarketing.Extracters
{
    public class EventConversionExtracter : Extracter<EventConversion>
    {
        private readonly DateRange dateRange;
        private readonly int advertiserId;
        private readonly int offerId;

        /// <summary>
        /// Action for exception of failed extraction.
        /// </summary>
        public event Action<CakeEventConversionsFailedEtlException> ProcessFailedExtraction;

        public EventConversionExtracter(DateRange dateRange, int advertiserId, int offerId)
        {
            this.dateRange = dateRange;
            this.advertiserId = advertiserId;
            this.offerId = offerId;
        }

        protected override void Extract()
        {
            Logger.Info($"Extracting EventConversions from {dateRange.FromDate:d} to {dateRange.ToDate:d}, AdvId {advertiserId} OffId {offerId}");
            foreach (var date in dateRange.Dates)
            {
                Extract(date);
            }
            End();
        }

        private void Extract(DateTime date)
        {
            try
            {
                LogInfoAboutCurrentDate(date);
                var singleDate = new DateRange(date, date.AddDays(1));
                var eventConvs = CakeMarketingUtility.EventConversions(singleDate, advertiserId, offerId);
                Add(eventConvs);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                var exception = new CakeEventConversionsFailedEtlException(date, date, advertiserId, offerId, e);
                ProcessFailedExtraction?.Invoke(exception);
            }
        }

        private void LogInfoAboutCurrentDate(DateTime date)
        {
            var logMessage = $"Extracting EventConversions for {date.ToShortDateString()}.";
            Logger.Info(logMessage);
            CommandExecutionContext.Current.SetJobExecutionStateInHistory(logMessage);
        }
    }
}
