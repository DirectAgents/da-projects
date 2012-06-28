using System;
using System.Collections.Generic;
using System.Linq;

namespace DAgents.Synch
{
    public partial class SynchDBDataContext : System.Data.Linq.DataContext
    {
        public SynchDBDataContext(bool b) :
            base(EomAppCommon.Settings.ConnStr, mappingSource)
        {
            OnCreated();
        }

        partial void OnCreated()
        {
            if (this.Connection.ConnectionString != EomAppCommon.Settings.ConnStr)
            {
                throw new Exception("Connection String Error");
            }
        }
    }

    partial class SynchDBDataContext
    {
        private List<PayoutEntity> payoutEntityList = new List<PayoutEntity>();

        public List<PayoutEntity> PayoutEntityList
        {
            get
            {
                if (payoutEntityList.Count == 0)
                {
                    payoutEntityList = (from c in PayoutEntities select c).ToList();
                }
                return payoutEntityList;
            }
            set { payoutEntityList = value; }
        }
    }

    partial class AffiliateEntity
    {
        internal static AffiliateEntity GetOrCreate(int affid, SynchDBDataContext db)
        {
            var aeq = from c in db.AffiliateEntities
                      where c.affid == affid
                      select c;

            var ae = aeq.FirstOrDefault();

            if (ae == null)
            {
                var db2 = new SynchDBDataContext(true);

                var ad = AffiliateDetail.Create(affid);

                ae = new AffiliateEntity();

                ae.affid = affid;
                ae.name = ad.CompanyName;
                ae.add_code = ad.AddCode;
                ae.email = ad.Email;

                if (affid == 462)
                {
                    ae.CurrencyEntity = (from c in db2.CurrencyEntities
                                         where c.name == "USD"
                                         select c).First();
                }
                else
                {
                    ae.CurrencyEntity = (from c in db2.CurrencyEntities
                                         where c.name == ad.PaymentCurrency
                                         select c).First();
                }

                ae.MediaBuyerEntity = (from c in db2.MediaBuyerEntities
                                       where c.name == "default"
                                       select c).First();

                ae.payment_method_id = 1; // default

                db2.AffiliateEntities.InsertOnSubmit(ae);

                db2.SubmitChanges();
            }

            return aeq.First();
        }
    }

    partial class StatEntity
    {
        internal static StatEntity GetOrCreateByApiUrl(string apiURL, SynchDBDataContext db)
        {
            StatEntity se = (from c in db.StatEntities
                             where c.apiurl == apiURL
                             select c).FirstOrDefault();

            if (se == null)
            {
                se = new StatEntity();
                se.apiurl = apiURL;
                db.StatEntities.InsertOnSubmit(se);
            }

            return se;
        }
    }

    partial class XmlSourceEntity
    {
        internal static XmlSourceEntity GetOrCreateByApiUrl(string apiURL, SynchDBDataContext db)
        {
            XmlSourceEntity xse = (from c in db.XmlSourceEntities
                                   where c.apiurl == apiURL
                                   select c).FirstOrDefault();

            if (xse == null)
            {
                xse = new XmlSourceEntity();
                xse.apiurl = apiURL;
                db.XmlSourceEntities.InsertOnSubmit(xse);
            }

            return xse;
        }
    }

    partial class PayoutEntity
    {
        internal static PayoutEntity GetOrCreate(string payoutID, SynchDBDataContext db)
        {
            PayoutEntity pe = (from c in db.PayoutEntities
                               where c.payout_id == payoutID
                               select c).FirstOrDefault();

            if (pe == null)
            {
                pe = new PayoutEntity();
                pe.payout_id = payoutID;
                db.PayoutEntities.InsertOnSubmit(pe);
            }

            return pe;
        }
    }

    partial class AdManagerEntity
    {
        internal static AdManagerEntity GetOrCreate(string personName)
        {
            var db = new SynchDBDataContext(true);

            AdManagerEntity ade = (from c in db.AdManagerEntities
                                   where c.name == personName
                                   select c).FirstOrDefault();
            if (ade == null)
            {
                ade = new AdManagerEntity();
                ade.name = personName;
                db.AdManagerEntities.InsertOnSubmit(ade);
                db.SubmitChanges();
            }

            return ade;
        }
    }

    partial class AccountManagerEntity
    {
        internal static AccountManagerEntity GetOrCreate(string personName)
        {
            var db = new SynchDBDataContext(true);

            AccountManagerEntity ame = (from c in db.AccountManagerEntities
                                        where c.name == personName
                                        select c).FirstOrDefault();
            if (ame == null)
            {
                ame = new AccountManagerEntity();
                ame.name = personName;
                db.AccountManagerEntities.InsertOnSubmit(ame);
                db.SubmitChanges();
            }

            return ame;
        }
    }

    partial class MediaBuyerEntity
    {
        internal static MediaBuyerEntity GetOrCreate(string personName)
        {
            var db = new SynchDBDataContext(true);

            MediaBuyerEntity mbe = (from c in db.MediaBuyerEntities
                                    where c.name == personName
                                    select c).FirstOrDefault();
            if (mbe == null)
            {
                mbe = new MediaBuyerEntity();
                mbe.name = personName;
                db.MediaBuyerEntities.InsertOnSubmit(mbe);
                db.SubmitChanges();
            }

            return mbe;
        }
    }

    partial class CampaignEntity
    {
        internal static CampaignEntity Update(int pid, SynchDBDataContext db)
        {
            CampaignEntity ce = (from c in db.CampaignEntities
                                 where c.pid == pid
                                 select c).First();

            // Get the campaign XML data from DirectTrack
            var dtCampaign = CampaignDetail.Create(pid);

            ce.campaign_name = dtCampaign.CampaignName;
            ce.campaign_type = dtCampaign.CampaignType;

            // These are fields that are here for the campaign "Wiki View"
            ce.dt_campaign_status = dtCampaign.CampaignStatus;
            ce.dt_campaign_url = dtCampaign.CampaignUrl;

            // Synch strategy - I'm deleting all and then up/inserting.
            string countryNames = string.Empty;
            foreach (var item in dtCampaign.AllowedCountries)
            {
                countryNames += ", " + item;
            }
            ce.dt_allowed_country_names = countryNames.TrimStart(',').TrimStart().ToUpper();

            db.SubmitChanges();

            return ce;
        }

        internal static CampaignEntity GetOrCreate(int pid, SynchDBDataContext db)
        {
            CampaignEntity ce = (from c in db.CampaignEntities
                                 where c.pid == pid
                                 select c).FirstOrDefault();
            if (ce == null)
            {
                ce = new CampaignEntity();
                ce.pid = pid;
                ce.account_manager_id = 1;
                ce.campaign_status_id = 1;
                ce.ad_manager_id = 1;
                ce.advertiser_id = 1;
                db.CampaignEntities.InsertOnSubmit(ce);
            }

            //db.SubmitChanges();

            return ce;
        }

        internal static CampaignEntity GetOrCreate(int pid, CampaignItem campaignItem)
        {
            SynchDBDataContext db = new SynchDBDataContext(true);

            CampaignEntity ce = (from c in db.CampaignEntities
                                 where c.pid == pid
                                 select c).FirstOrDefault();
            if (ce == null)
            {
                ce = new CampaignEntity();

                ce.pid = pid;

                ce.campaign_name = campaignItem.CampaignName;

                ce.account_manager_id = 1;

                ce.campaign_status_id = 1;

                ce.ad_manager_id = 1;

                ce.advertiser_id = 1;

                db.CampaignEntities.InsertOnSubmit(ce);
            }

            db.SubmitChanges();

            return ce;
        }

        internal static CampaignEntity UpdateOrCreate(SynchDBDataContext db, CampaignItem campaignItem, out bool created)
        {
            int pid = campaignItem.CampaignId;
            CampaignEntity entity = (from c in db.CampaignEntities
                                     where c.pid == pid
                                     select c).FirstOrDefault();
            if (entity == null)
            {
                entity = new CampaignEntity();
                entity.pid = pid;
                created = true;
            }
            else
            {
                created = false;
            }
            entity.campaign_name = campaignItem.CampaignName;
            return entity;
        }

        internal static void UpdateName(SynchDBDataContext db, CampaignItem campaignItem)
        {
            int pid = campaignItem.CampaignId;
            CampaignEntity entity = (from c in db.CampaignEntities
                                     where c.pid == pid
                                     select c).FirstOrDefault();
            if (entity != null && campaignItem.CampaignName != entity.campaign_name)
            {
                entity.campaign_name = campaignItem.CampaignName;
                //db.SubmitChanges();
            }
        }

        internal static CampaignEntity Get(int pid, SynchDBDataContext db)
        {
            CampaignEntity ce = (from c in db.CampaignEntities
                                 where c.pid == pid
                                 select c).FirstOrDefault();
            if (ce == null)
            {
                throw new Exception("campaign with pid " + pid + " does not exist");
            }

            return ce;
        }
    }
}
