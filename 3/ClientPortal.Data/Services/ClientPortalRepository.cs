using System;
using System.Collections.Generic;
using System.Linq;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;
using System.Data;

namespace ClientPortal.Data.Services
{
    public class ClientPortalRepository : IClientPortalRepository
    {
        ClientPortalContext context;

        public ClientPortalRepository(ClientPortalContext clientPortalContext)
        {
            this.context = clientPortalContext;
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        #region ConversionData
        public void AddConversionData(ConversionData entity)
        {
            context.ConversionDatas.Add(entity);
        }

        public IQueryable<ConversionData> ConversionData
        {
            get { return context.ConversionDatas; }
        }
        #endregion

        public IQueryable<Offer> Offers(int? advertiserId)
        {
            return context.Offers.Where(o => o.Advertiser_Id == advertiserId);
        }

        public IQueryable<DailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? advertiserId, int? offerId, out string currency)
        {
            var dailySummaries = context.DailySummaries.AsQueryable();
            if (start.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date >= start);
            if (end.HasValue) dailySummaries = dailySummaries.Where(ds => ds.date <= end);

            var offers = Offers(advertiserId);

            if (offerId.HasValue)
            {
                offers = offers.Where(o => o.Offer_Id == offerId.Value);
                dailySummaries = dailySummaries.Where(ds => ds.offer_id == offerId.Value);
            }
            else
            {
                dailySummaries = from ds in dailySummaries
                                 join o in offers on ds.offer_id equals o.Offer_Id
                                 select ds;
            }
            currency = null; // Assume all offers for the advertiser have the same currency
            if (offers.Any()) currency = offers.First().Currency;

            return dailySummaries;
        }

        public DateRangeSummary GetDateRangeSummary(DateTime? start, DateTime? end, int? advertiserId, int? offerId, bool includeConversionData)
        {
            // * remove this when ready! *
            return GetDateRangeSummaryOld(start, end, advertiserId, offerId, includeConversionData);

            string currency;
            var dailySummaries = GetDailySummaries(start, end, advertiserId, offerId, out currency);

            var any = dailySummaries.Any();
            DateRangeSummary summary = new DateRangeSummary()
            {
                Clicks = any ? dailySummaries.Sum(ds => ds.clicks) : 0,
                Conversions = any ? dailySummaries.Sum(ds => ds.conversions) : 0,
                Revenue = any ? dailySummaries.Sum(ds => ds.revenue) : 0,
                Currency = currency
            };
            if (includeConversionData)
            {
                var conversions = GetConversions(start, end, advertiserId, offerId);
                var conv_datas =
                    from c in conversions
                    join conv_data in context.ConversionDatas on c.conversion_id equals conv_data.conversion_id
                    select conv_data;

                summary.ConVal = conv_datas.Any() ? conv_datas.Sum(c => c.value0) : 0;
            }
            return summary;
        }

        private DateRangeSummary GetDateRangeSummaryOld(DateTime? start, DateTime? end, int? advertiserId, int? offerId, bool includeConversionData)
        {
            int? advId = advertiserId;

            var clicks = GetClicks(start, end, advId, offerId);
            var conversions = GetConversions(start, end, advId, offerId);

            var anyConv = conversions.Any();
            DateRangeSummary summary = new DateRangeSummary()
            {
                Clicks = clicks.Count(),
                Conversions = anyConv ? conversions.Count() : 0,
                Revenue = anyConv ? conversions.Sum(c => c.received_amount) : 0,
                Currency = null // TODO: determine this... need Offers in ClientPortalContext
            };
            if (includeConversionData)
            {
                var conv_datas =
                    from c in conversions
                    join conv_data in context.ConversionDatas on c.conversion_id equals conv_data.conversion_id
                    select conv_data;

                summary.ConVal = conv_datas.Any() ? conv_datas.Sum(c => c.value0) : 0;
            }
            return summary;
        }

        public IQueryable<ConversionInfo> GetConversionInfos(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var conversions = GetConversions(start, end, advertiserId, offerId);

            var conversionInfos =
                from c in conversions
                join conv_data in context.ConversionDatas on c.conversion_id equals conv_data.conversion_id into gj
                from cd in gj.DefaultIfEmpty() // left join to ConversionData
                //                join curr in cakeContext.CakeCurrencies on c.PriceReceivedCurrencyId.Value equals curr.Id
                //                join offer in cakeContext.CakeOffers on c.Offer_Id.Value equals offer.Offer_Id into gj
                //                from o in gj.DefaultIfEmpty() // left join to CakeOffers
                select new ConversionInfo()
                {
                    ConversionIdString = c.conversion_id,
                    Date = c.conversion_date,
                    AffId = c.affiliate_id,
                    OfferId = c.offer_id,
                    Offer = String.Empty,
                    //                    Offer = (o == null) ? String.Empty : o.OfferName,
                    PriceReceived = c.received_amount,
                    Currency = null,
                    //                    Currency = curr.Name,
                    TransactionId = c.transaction_id,
                    //                    Positive = c.Positive,
                    ConVal = (cd == null) ? 0 : cd.value0
                };
            return conversionInfos;
        }

        // get clicks through 23:59:59 on the "end" date
        public IQueryable<Click> GetClicks(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var clicks = context.Clicks.AsQueryable();
            if (start.HasValue)
                clicks = clicks.Where(c => c.click_date >= start.Value);
            if (end.HasValue)
            {
                DateTime endOfDay = new DateTime(end.Value.Year, end.Value.Month, end.Value.Day, 23, 59, 59);
                clicks = clicks.Where(c => c.click_date <= endOfDay);
            }
            if (advertiserId.HasValue)
                clicks = clicks.Where(c => c.advertiser_id == advertiserId.Value);
            if (offerId.HasValue)
                clicks = clicks.Where(c => c.offer_id == offerId.Value);

            return clicks;
        }

        // get conversions through 23:59:59 on the "end" date
        public IQueryable<Conversion> GetConversions(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var conversions = context.Conversions.AsQueryable();
            if (start.HasValue)
                conversions = conversions.Where(c => c.conversion_date >= start.Value);
            if (end.HasValue)
            {
                DateTime endOfDay = new DateTime(end.Value.Year, end.Value.Month, end.Value.Day, 23, 59, 59);
                conversions = conversions.Where(c => c.conversion_date <= endOfDay);
            }
            if (advertiserId.HasValue)
                conversions = conversions.Where(c => c.advertiser_id == advertiserId.Value);
            if (offerId.HasValue)
                conversions = conversions.Where(c => c.offer_id == offerId.Value);

            return conversions;
        }

        public static int? ParseInt(string stringVal)
        {
            int intVal;
            if (Int32.TryParse(stringVal, out intVal))
                return intVal;
            else
                return null;
        }

        public IEnumerable<DeviceClicks> GetClicksByDeviceName(DateTime? start, DateTime? end, int? advertiserId, int? offerId)
        {
            var query = (
                from click in GetClicks(start, end, advertiserId, offerId)
                group click by click.device_name into g
                orderby g.Count() descending
                select new
                {
                    Device = g.Key,
                    Count = g.Count()
                })
                // we return IEnumerable (as opposed to IQueryable) bec. AsEnumerable() is called
                .AsEnumerable().Select(c => new DeviceClicks 
                {
                    DeviceName = string.IsNullOrWhiteSpace(c.Device) ? "Other" : c.Device,
                    ClickCount = c.Count
                });
            return query;
        }

        #region Advertisers & Contacts
        public IQueryable<Advertiser> Advertisers
        {
            get { return context.Advertisers; }
        }
        public IQueryable<Contact> Contacts
        {
            get { return context.Contacts; }
        }

        public void AddAdvertiser(Advertiser entity)
        {
            context.Advertisers.Add(entity);
        }
        public void AddContact(Contact entity)
        {
            context.Contacts.Add(entity);
        }

        public Advertiser GetAdvertiser(int id)
        {
            var advertiser = context.Advertisers.Find(id);
            return advertiser;
        }
        public Contact GetContact(string search) // search by last name, for now
        {
            var contact = context.Contacts.Where(c => c.LastName == search).FirstOrDefault();
            return contact;
        }

        #endregion

        #region ScheduledReports
        public IQueryable<ScheduledReport> GetScheduledReports(int advertiserId)
        {
            var scheduledReports = context.ScheduledReports.Where(sr => sr.AdvertiserId == advertiserId);
            return scheduledReports;
        }

        public ScheduledReport GetScheduledReport(int id)
        {
            var rep = context.ScheduledReports.Find(id);
            return rep;
        }

        public void AddScheduledReport(ScheduledReport scheduledReport)
        {
            context.ScheduledReports.Add(scheduledReport);
        }

        //public void SaveScheduledReport(ScheduledReport scheduledReport)
        //{
        //    var entry = context.Entry(scheduledReport);
        //    entry.State = EntityState.Modified;
        //    context.SaveChanges();
        //}

        public bool DeleteScheduledReport(int id, int? advertiserId)
        {
            bool deleted = false;
            var rep = context.ScheduledReports.Find(id);
            if (rep != null && (advertiserId == null || rep.AdvertiserId == advertiserId.Value))
            {
                context.ScheduledReports.Remove(rep);
                context.SaveChanges();
                deleted = true;
            }
            return deleted;
        }
        #endregion

        #region Files
        public IQueryable<FileUpload> GetFileUploads(int? advertiserId)
        {   // Note: if advertiserId == null, return the FileUploads where advertiserId is null
            var fileUploads = context.FileUploads.Where(f => f.AdvertiserId == advertiserId);
            return fileUploads;
        }

        public FileUpload GetFileUpload(int id)
        {
            var fileUpload = context.FileUploads.Find(id);
            return fileUpload;
        }

        public void AddFileUpload(FileUpload fileUpload, bool saveChanges = false)
        {
            context.FileUploads.Add(fileUpload);
            if (saveChanges) SaveChanges();
        }

        public void DeleteFileUpload(FileUpload fileUpload, bool saveChanges = false)
        {
            context.FileUploads.Remove(fileUpload);
            if (saveChanges) SaveChanges();
        }
        #endregion

        #region Goals
        public IQueryable<Goal> GetGoals(int advertiserId)
        {
            var goals = context.Goals.Where(g => g.AdvertiserId == advertiserId);
            return goals;
        }

        public Goal GetGoal(int id)
        {
            var goal = context.Goals.Find(id);
            return goal;
        }

        public bool DeleteGoal(int id, int? advertiserId)
        {
            bool deleted = false;
            var goal = context.Goals.Find(id);
            if (goal != null && (advertiserId == null || goal.AdvertiserId == advertiserId.Value))
            {
                context.Goals.Remove(goal);
                context.SaveChanges();
                deleted = true;
            }
            return deleted;
        }
        #endregion
    }
}
