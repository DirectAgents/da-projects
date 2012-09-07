using System;
using System.Linq;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Concrete
{
    public class MainRepository : IMainRepository
    {
        EomEntities context;

        public MainRepository(EomEntities context)
        {
            this.context = context;
        }

        public IQueryable<PublisherPayout> PublisherPayouts
        {
            get { return context.PublisherPayouts; }
        }

        public IQueryable<CampaignsPublisherReportDetail> CampaignPublisherReportDetails
        {
            get { return context.CampaignsPublisherReportDetails; }
        }

        public IQueryable<PublisherPayout> PublisherPayoutsByMode(string mode, bool includeZero)
        {
            var payouts = this.PublisherPayouts;
            if (!includeZero) payouts = payouts.Where(p => p.Pub_Payout > 0);

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
                var pubSummaries = PubPayoutsToPubSummaries(this.PublisherPayouts);
                return pubSummaries;
            }
        }

        public IQueryable<PublisherSummary> PublisherSummariesByMode(string mode, bool includeZero)
        {
            var pubPayouts = PublisherPayoutsByMode(mode, includeZero);
            var pubSummaries = PubPayoutsToPubSummaries(pubPayouts);
            return pubSummaries;
        }
        private IQueryable<PublisherSummary> PubPayoutsToPubSummaries(IQueryable<PublisherPayout> publisherPayouts)
        {
            var pubSummaries = publisherPayouts
                .GroupBy(p => new { affid = p.affid, Publisher = p.Publisher, Currency = p.Pub_Pay_Curr }).ToList()
                .Select(p => new PublisherSummary
                {
                    affid = p.Key.affid,
                    PublisherName = p.Key.Publisher,
                    Currency = p.Key.Currency,
                    PayoutTotal = p.Sum(pp => pp.Pub_Payout) ?? 0,
                    MinPctMargin = p.Min(pp => pp.MarginPct) ?? 0,
                    MaxPctMargin = p.Max(pp => pp.MarginPct) ?? 0,
                    BatchIds = String.Join(",", String.Join(",", p.Select(pp => pp.BatchIds)).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct())
                }).ToList();

            // Get latest note for each PublisherSummary
            var batchIds = String.Join(",", pubSummaries.Select(ps => ps.BatchIds)).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32(id)).Distinct().ToList();
            var batchesList = context.Batches.Include("BatchUpdates").Where(b => batchIds.Contains(b.id)).ToList();
            for (var i=0; i < pubSummaries.Count(); i++)
            {
                var pubSummary = pubSummaries[i];
                var batches = batchesList.Where(b => pubSummary.BatchIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32(id)).Contains(b.id));
                var latestBatchUpdate = batches.SelectMany(b => b.BatchUpdates).Distinct().OrderByDescending(bu => bu.date_created).FirstOrDefault();
                if (latestBatchUpdate != null)
                    pubSummary.LatestNote = latestBatchUpdate.note;
            }
            return pubSummaries.AsQueryable();
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
        public void ApproveItemsByAffId(int affId, string note, string author)
        {
            SetMediaBuyerApprovalStatus(affId, note, author, MediaBuyerApprovalStatus.Approved, MediaBuyerApprovalStatus.Sent);
        }

        // only release finalized items that are on hold
        public void ReleaseItemsByAffId(int affId, string note, string author)
        {
            SetMediaBuyerApprovalStatus(affId, note, author, MediaBuyerApprovalStatus.Approved, MediaBuyerApprovalStatus.Hold);
        }

        // only hold finalized items that are "Sent"
        public void HoldItemsByAffId(int affId, string note, string author)
        {
            SetMediaBuyerApprovalStatus(affId, note, author, MediaBuyerApprovalStatus.Hold, MediaBuyerApprovalStatus.Sent);
        }

        private void SetMediaBuyerApprovalStatus(int affId, string note, string author, int toMediaBuyerApprovalStatus, int fromMediaBuyerApprovalStatus)
        {
            var items = context.Items.Where(i => i.affid == affId &&
                                            i.campaign_status_id == CampaignStatus.Finalized &&
                                            i.media_buyer_approval_status_id == fromMediaBuyerApprovalStatus);
            foreach (var item in items)
            {
                item.media_buyer_approval_status_id = toMediaBuyerApprovalStatus;
            }
            string extra = null;
            var aff = context.Affiliates.Where(a => a.affid == affId);
            if (aff.Count() > 0)
                extra = aff.First().name2;
            SetNoteForItems(items, note, author, toMediaBuyerApprovalStatus, extra);
            context.SaveChanges();
        }
        private void SetNoteForItems(IQueryable<Item> items, string note, string author, int mediaBuyerApprovalStatus, string extra)
        {
            var batches = items.Where(i => i.Batch != null).Select(i => i.Batch).Distinct().ToList();
            var itemsWithNoBatch = items.Where(i => i.batch_id == null);
            if (itemsWithNoBatch.Count() > 0)
            {
                var newBatch = new Batch();
                foreach (var item in itemsWithNoBatch)
                    item.Batch = newBatch;
                batches.Add(newBatch);
            }
            var batchUpdate = new BatchUpdate()
            {
                note = note,
                author = author,
                media_buyer_approval_status_id = mediaBuyerApprovalStatus,
                extra = extra
            };
            foreach (var batch in batches)
            {
                batch.BatchUpdates.Add(batchUpdate);
            }
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
