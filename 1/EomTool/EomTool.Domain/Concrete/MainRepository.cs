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

        public IQueryable<PublisherSummary> PublisherSummaries
        {
            get
            {
                return PublisherPayouts.GroupBy(p => new { affid = p.affid, Publisher = p.Publisher, Currency = p.Pub_Pay_Curr }).Select(p => new PublisherSummary { affid = p.Key.affid, PublisherName = p.Key.Publisher, Currency = p.Key.Currency, PayoutTotal = p.Sum(pp => pp.Pub_Payout) ?? 0 });
                //PublisherPayouts.ToLookup(p => p.Publisher,
            }
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

        public IQueryable<Affiliate> AffiliatesForMediaBuyers(string[] mediaBuyers)
        {
            var affs = from a in context.Affiliates
                       where mediaBuyers.Contains(a.MediaBuyer.name)
                       select a;
            return affs;
        }
    }
}
