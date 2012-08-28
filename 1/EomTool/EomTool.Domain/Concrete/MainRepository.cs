using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Concrete
{
    public class MainRepository : IMainRepository
    {
        EomEntities context;
        public MainRepository()
        {
            context = EomEntities.Create();
        }

        public IQueryable<PublisherPayout> PublisherPayouts
        {
            get { return context.PublisherPayouts; }
        }

        public IQueryable<CampaignsPublisherReportDetail> CampaignPublisherReportDetails
        {
            get { return context.CampaignsPublisherReportDetails; }
        }

        public IQueryable<PublisherPayout> PublisherPayoutsByMode(string mode)
        {
            var payouts = this.PublisherPayouts;

            if (mode == "preapproval")
            {
                payouts = payouts.Where(p => p.status_id == CampaignStatus.Default || p.status_id == CampaignStatus.Active
                    || (p.status_id == CampaignStatus.Finalized &&
                    (p.media_buyer_approval_status_id == MediaBuyerApprovalStatus.Default || p.media_buyer_approval_status_id == MediaBuyerApprovalStatus.Queued)));
            }
            else if (mode == "held")
            {
                payouts = payouts.Where(p => p.status_id == CampaignStatus.Finalized && p.media_buyer_approval_status_id == MediaBuyerApprovalStatus.Hold);
            }
            else if (mode == "approved")
            {
                payouts = payouts.Where(p => (p.status_id == CampaignStatus.Finalized && p.media_buyer_approval_status_id == MediaBuyerApprovalStatus.Approved)
                    || p.status_id == CampaignStatus.Verified);
            }
            else // normal view - show payouts needing action
            {
                payouts = payouts.Where(p => p.status_id == CampaignStatus.Finalized && p.media_buyer_approval_status_id == MediaBuyerApprovalStatus.Sent);
            }

            return payouts;
        }

        public IQueryable<PublisherSummary> PublisherSummaries
        {
            get
            {
                return PublisherPayouts
                    .GroupBy(p => new { affid = p.affid, Publisher = p.Publisher, Currency = p.Pub_Pay_Curr })
                    .Select(p => new PublisherSummary { affid = p.Key.affid, PublisherName = p.Key.Publisher, Currency = p.Key.Currency, PayoutTotal = p.Sum(pp => pp.Pub_Payout) ?? 0 });
            }
        }

        public IQueryable<PublisherSummary> PublisherSummariesByMode(string mode)
        {
            var result =
                PublisherPayoutsByMode(mode)
                    .Where(c => c.Pub_Payout > 0)
                    .GroupBy(p => new { affid = p.affid, Publisher = p.Publisher, Currency = p.Pub_Pay_Curr })
                    .Select(p => new PublisherSummary
                    {
                        affid = p.Key.affid,
                        PublisherName = p.Key.Publisher,
                        Currency = p.Key.Currency,
                        PayoutTotal = p.Sum(pp => pp.Pub_Payout) ?? 0,
                        MinPctMargin = p.Min(pp => pp.MarginPct) ?? 0,
                        MaxPctMargin = p.Max(pp => pp.MarginPct) ?? 0,
                    });
            return result;
        }

        public void Media_ApproveItems(int[] itemIds)
        {
            var items = context.Items.Where(item => itemIds.Contains(item.id));
            foreach (var item in items)
            {
                item.media_buyer_approval_status_id = MediaBuyerApprovalStatus.Approved;
            }
            context.SaveChanges();
        }

        public void Media_HoldItems(int[] itemIds)
        {
            var items = context.Items.Where(item => itemIds.Contains(item.id));
            foreach (var item in items)
            {
                item.media_buyer_approval_status_id = MediaBuyerApprovalStatus.Hold;
            }
            context.SaveChanges();
        }

        // only approve finalized items that are "Sent"
        public void ApproveItemsByAffId(int affId)
        {
            var items = context.Items.Where(i => i.affid == affId &&
                                            i.campaign_status_id == CampaignStatus.Finalized &&
                                            i.media_buyer_approval_status_id == MediaBuyerApprovalStatus.Sent);
            foreach (var item in items)
            {
                item.media_buyer_approval_status_id = MediaBuyerApprovalStatus.Approved;
            }
            context.SaveChanges();
        }

        // only hold finalized items that are "Sent"
        public void HoldItemsByAffId(int affId)
        {
            var items = context.Items.Where(i => i.affid == affId &&
                                            i.campaign_status_id == CampaignStatus.Finalized &&
                                            i.media_buyer_approval_status_id == MediaBuyerApprovalStatus.Sent);
            foreach (var item in items)
            {
                item.media_buyer_approval_status_id = MediaBuyerApprovalStatus.Hold;
            }
            context.SaveChanges();
        }

        public IQueryable<Affiliate> AffiliatesForMediaBuyers(string[] mediaBuyers)
        {
            var affs = from a in context.Affiliates
                       where mediaBuyers.Contains(a.MediaBuyer.name)
                       select a;
            return affs;
        }
    }
}
