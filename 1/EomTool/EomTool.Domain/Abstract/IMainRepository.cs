﻿using System.Linq;
using EomTool.Domain.Entities;
using EomTool.Domain.DTOs;
using System.Collections.Generic;

namespace EomTool.Domain.Abstract
{
    public interface IMainRepository
    {
        void SaveChanges();

        Advertiser GetAdvertiser(int advId);

        IQueryable<AccountManager> AccountManagers(bool withActivityOnly = false);
        IQueryable<Campaign> Campaigns(int? accountManagerId, int? advertiserId, bool activeOnly = false);

        IQueryable<CampaignAmount> CampaignAmounts(int? accountManagerId, int? advertiserId, bool byAffiliate = false);
        IEnumerable<CampaignAmount> CampaignAmounts(IEnumerable<CampAffId> campAffIds);
        Invoice GenerateInvoice(IEnumerable<CampAffId> campAffIds);

        // --- Extra Item Import stuff ---
        bool CampaignExists(int pid);
        Campaign GetCampaign(int pid);

        bool AffiliateExists(int affId);
        Affiliate GetAffiliate(int affId);

        Source GetSource(int sourceId);
        Source GetSource(string sourceName);

        bool UnitTypeExists(int unitTypeId);
        UnitType GetUnitType(int unitTypeId);
        UnitType GetUnitType(string unitTypeName);

        bool CurrencyExists(int currency);
        Currency GetCurrency(string currencyName);

        void AddItem(Item item);
        bool ItemExists(Item item);

        // --- Media Buyer Approval stuff ---
        IQueryable<PublisherPayout> PublisherPayouts { get; }
        IQueryable<PublisherSummary> PublisherSummaries { get; }
        IQueryable<PublisherPayout> PublisherPayoutsByMode(string mode, bool includeZero);
        IQueryable<PublisherSummary> PublisherSummariesByMode(string mode, bool includeZero);
        IQueryable<CampaignsPublisherReportDetail> CampaignPublisherReportDetails { get; }

        void Media_ApproveItems(int[] itemIds);
        void Media_HoldItems(int[] itemIds);
        void ApproveItemsByAffId(int affId, string note, string author);
        void ReleaseItemsByAffId(int affId, string note, string author);
        void HoldItemsByAffId(int affId, string note, string author);

        IQueryable<Affiliate> AffiliatesForMediaBuyers(string[] mediaBuyers);
    }
}
