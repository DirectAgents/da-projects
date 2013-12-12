using System;
using System.Collections.Generic;
using System.Linq;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs;

namespace ClientPortal.Data.Services
{
    public partial class ClientPortalRepository : IClientPortalRepository
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

        // ------

        #region Offers, Campaigns, Affiliates
        public IQueryable<Offer> Offers(int? advertiserId)
        {
            return context.Offers.Where(o => o.AdvertiserId == advertiserId);
        }

        // includes Advertisers
        public IQueryable<Offer> Offers(bool cpmOnly, int? minCampaigns = null)
        {
            var offers = context.Offers.AsQueryable();
            if (cpmOnly)
                offers = offers.Where(o => o.OfferName.Contains("CPM"));
            if (minCampaigns.HasValue)
                offers = offers.Where(o => o.Campaigns.Count >= minCampaigns);

            var q = from o in offers
                    join a in context.Advertisers.AsQueryable()
                    on o.AdvertiserId equals a.AdvertiserId
                    select new { Offer = o, AdvertiserId = a.AdvertiserId, AdvertiserName = a.AdvertiserName };

            var off = from row in q.AsEnumerable()
                      select row.Offer.ThisWithAdvertiserInfo(row.AdvertiserId, row.AdvertiserName);

            return off.AsQueryable();
        }

        public Offer GetOffer(int id)
        {
            var offer = context.Offers.Find(id);
            return offer;
        }

        // use offerId==null for all campaigns
        public IQueryable<Campaign> Campaigns(int? offerId, bool cpmOnly)
        {
            var campaigns = context.Campaigns.Include("Affiliate").AsQueryable();

            if (offerId.HasValue)
                campaigns = campaigns.Where(c => c.OfferId == offerId.Value);
            if (cpmOnly)
                campaigns = campaigns.Where(c => c.Offer.OfferName.Contains("CPM"));

            return campaigns;
        }

        public Campaign GetCampaign(int id)
        {
            var campaign = context.Campaigns.Find(id);
            return campaign;
        }
        #endregion

        #region CampaignDrops
        public IQueryable<CampaignDrop> CampaignDrops(int? offerId, int? campaignId)
        {
            var campaignDrops = context.CampaignDrops.Include("Campaign").Include("CreativeStats.Creative").AsQueryable();
            if (offerId.HasValue)
                campaignDrops = campaignDrops.Where(cd => cd.Campaign.OfferId == offerId.Value);
            if (campaignId.HasValue)
                campaignDrops = campaignDrops.Where(cd => cd.CampaignId == campaignId.Value);
            return campaignDrops;
        }

        public CampaignDrop GetCampaignDrop(int id)
        {
            var campaignDrop = context.CampaignDrops.Find(id);
            return campaignDrop;
        }

        public CampaignDrop AddCampaignDrop(int campaignId, DateTime date, int creativeId, bool saveChanges = false)
        {
            CampaignDrop campaignDrop = null;
            var campaign = context.Campaigns.Find(campaignId);
            var creative = context.Creatives.Find(creativeId);
            if (campaign != null && creative != null)
            {
                campaignDrop = new CampaignDrop
                {
                    Campaign = campaign,
                    Date = date
                };
                var creativeStat = new CreativeStat
                {
                    Creative = creative,
                    CampaignDrop = campaignDrop
                };
                context.CreativeStats.Add(creativeStat);

                if (saveChanges) SaveChanges();
            }
            return campaignDrop;
        }

        public bool SaveCampaignDrop(CampaignDrop inDrop, bool saveChanges)
        {
            bool success = false;
            var drop = context.CampaignDrops.Find(inDrop.CampaignDropId);
            if (drop != null)
            {
                success = true;

                drop.Date = inDrop.Date;
                drop.Subject = inDrop.Subject;
                drop.Cost = inDrop.Cost;
                drop.Volume = inDrop.Volume;
                drop.Opens = inDrop.Opens;
                foreach(var creativeStat in inDrop.CreativeStats)
                {
                    bool saved = SaveCreativeStat(creativeStat);
                    if (!saved)
                        success = false;
                }
                if (success && saveChanges) SaveChanges();
            }
            return success;
        }

        public void FillExtended_CampaignDrop(CampaignDrop drop)
        {
            if (drop.Campaign == null)
            {
                var campaignDrop = context.CampaignDrops.Find(drop.CampaignDropId);
                if (campaignDrop != null)
                {
                    drop.CampaignId = campaignDrop.CampaignId;
                    drop.Campaign = campaignDrop.Campaign;
                }
            }
        }

        private bool SaveCreativeStat(CreativeStat inStat)
        {
            bool success = false;
            var stat = context.CreativeStats.Find(inStat.CreativeStatId);
            if (stat != null)
            {
                stat.CreativeId = inStat.CreativeId;
                stat.Clicks = inStat.Clicks;
                stat.Leads = inStat.Leads;

                success = true;
            }
            return success;
        }
        #endregion

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
                for (int i = rep.ScheduledReportRecipients.Count - 1; i >= 0; i--)
                {
                    var recipient = rep.ScheduledReportRecipients.ElementAt(i);
                    context.ScheduledReportRecipients.Remove(recipient);
                }
                context.ScheduledReports.Remove(rep);
                context.SaveChanges();
                deleted = true;
            }
            return deleted;
        }

        public void DeleteScheduledReportRecipient(ScheduledReportRecipient recipient)
        {
            context.ScheduledReportRecipients.Remove(recipient);
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
        public IQueryable<Goal> Goals
        {
            get { return context.Goals; }
        }

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

        public void AddGoal(Goal goal, bool saveChanges = false)
        {
            context.Goals.Add(goal);
            if (saveChanges) SaveChanges();
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

        #region UserEvents
        public IQueryable<UserEvent> UserEvents
        {
            get { return context.UserEvents; }
        }

        public void AddUserEvent(UserEvent userEvent, bool saveChanges = false)
        {
            context.UserEvents.Add(userEvent);
            if (saveChanges) SaveChanges();
        }

        public void AddUserEvent(string userName, string eventString, bool saveChanges = false)
        {
            var userProfile = context.UserProfiles.SingleOrDefault(up => up.UserName == userName);
            if (userProfile != null)
                AddUserEvent_NoCheck(userProfile.UserId, eventString, saveChanges);
            else
                AddUserEvent_NoCheck(null, eventString + " [" + userName + "]", saveChanges);
        }

        public void AddUserEvent(int userId, string eventString, bool saveChanges = false)
        {
            var userProfile = context.UserProfiles.SingleOrDefault(up => up.UserId == userId);
            if (userProfile != null)
                AddUserEvent_NoCheck(userProfile.UserId, eventString, saveChanges);
            else
                AddUserEvent_NoCheck(null, eventString + "[id: " + userId + "]", saveChanges);
        }

        private void AddUserEvent_NoCheck(int? userId, string eventString, bool saveChanges)
        {
            var userEvent = new UserEvent
            {
                UserId = userId,
                Event = eventString
            };
            context.UserEvents.Add(userEvent);
            if (saveChanges) SaveChanges();
        }
        #endregion

        // --- Helpers ---
        public static int? ParseInt(string stringVal)
        {
            int intVal;
            if (Int32.TryParse(stringVal, out intVal))
                return intVal;
            else
                return null;
        }
    }
}
