﻿using System;
using System.Linq;
using DirectAgents.Domain.Entities.AdRoll;
using DirectAgents.Domain.Entities.DBM;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.Abstract
{
    public interface ITDRepository : IDisposable
    {
        void SaveChanges();

        // TD
        IQueryable<Advertiser> Advertisers();
        IQueryable<Campaign> Campaigns(int? advId = null);
        IQueryable<Account> Accounts(string platformCode = null);
        IQueryable<DailySummary> DailySummaries(DateTime? startDate, DateTime? endDate, int? accountId = null);
        TDStatWithAccount GetTDStatWithAccount(DateTime? startDate, DateTime? endDate, Account account = null);

        // AdRoll
        Advertisable Advertisable(string eid);
        IQueryable<Advertisable> Advertisables();
        IQueryable<Ad> AdRoll_Ads(int? advId = null, string advEid = null);
        IQueryable<AdDailySummary> AdRoll_AdDailySummaries(int? advertisableId, int? adId, DateTime? startDate, DateTime? endDate);
        TDStat GetAdRollStat(Ad ad, DateTime? startDate, DateTime? endDate);

        // DBM
        InsertionOrder InsertionOrder(int ioID);
        IQueryable<InsertionOrder> InsertionOrders();
        IQueryable<Creative> DBM_Creatives(int? ioID);
        IQueryable<CreativeDailySummary> DBM_CreativeDailySummaries(DateTime? startDate, DateTime? endDate, int? ioID = null, int? creativeID = null);
        TDStat GetDBMStat(Creative creative, DateTime? startDate, DateTime? endDate);
    }
}
