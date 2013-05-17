using System;
using System.Collections.Generic;
using System.Linq;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;

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

        public void AddConversionData(ConversionData entity)
        {
            context.ConversionDatas.Add(entity);
        }

        public IQueryable<ConversionData> ConversionData
        {
            get { return context.ConversionDatas; }
        }

        public DateRangeSummary GetDateRangeSummary(DateTime? start, DateTime? end, string advertiserId, int? offerId, bool includeConversionData)
        {
            int? advId = ParseInt(advertiserId);

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
            var advertiser = context.Advertisers.Where(a => a.AdvertiserId == id).FirstOrDefault();
            return advertiser;
        }
        public Contact GetContact(string search) // search by last name, for now
        {
            var contact = context.Contacts.Where(c => c.LastName == search).FirstOrDefault();
            return contact;
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
            var goal = context.Goals.Where(g => g.Id == id).FirstOrDefault();
            return goal;
        }

        public bool DeleteGoal(int id, int? advertiserId)
        {
            bool deleted = false;
            var goal = context.Goals.Where(g => g.Id == id).FirstOrDefault();
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
