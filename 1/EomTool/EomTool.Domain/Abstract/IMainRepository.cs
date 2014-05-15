﻿using System.Linq;
using EomTool.Domain.Entities;
using EomTool.Domain.DTOs;
using System.Collections.Generic;

namespace EomTool.Domain.Abstract
{
    public interface IMainRepository
    {
        void SaveChanges();

        AccountManagerTeam GetAccountManagerTeam(int id);
        Advertiser GetAdvertiser(int id);

        IQueryable<AccountManagerTeam> AccountManagerTeams(bool withActivityOnly = false);
        IQueryable<Campaign> Campaigns(int? amId, int? advertiserId, bool activeOnly = false);

        IEnumerable<CampaignAmount> CampaignAmounts(int? amId, int? advertiserId, bool byAffiliate, int? campaignStatus);

        Invoice GenerateInvoice(IEnumerable<CampAffId> campAffIds);
        void SaveInvoice(Invoice invoice, bool markSentToAccounting = false);
        IQueryable<Invoice> Invoices(bool fillExtended);
        Invoice GetInvoice(int id, bool fillLineItems = false);
        bool SetInvoiceStatus(int id, int statusId);

        // ---

        string UnitTypeName(int unitTypeId);
        string ItemCode(int unitTypeId);
        bool UnitTypeExists(int unitTypeId);
        UnitType GetUnitType(int unitTypeId);
        UnitType GetUnitType(string unitTypeName);

        bool CurrencyExists(int currency);
        string CurrencyName(int currencyId);
        Currency GetCurrency(int currencyId);
        Currency GetCurrency(string currencyName);

        // --- Extra Item Import stuff ---
        bool CampaignExists(int pid);
        Campaign GetCampaign(int pid);
        bool SaveCampaign(Campaign inCampaign);

        bool AffiliateExists(int affId);
        Affiliate GetAffiliate(int affId);

        Source GetSource(int sourceId);
        Source GetSource(string sourceName);

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
