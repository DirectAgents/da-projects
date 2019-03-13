namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_CjDataTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.CjAdvertiserCommissionItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommissionId = c.Int(),
                        DiscountUsd = c.String(),
                        ItemListId = c.String(),
                        PerItemSaleAmountUsd = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Quantity = c.Int(nullable: false),
                        Sku = c.String(),
                        TotalCommissionUsd = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.CjAdvertiserCommission", t => t.CommissionId)
                .Index(t => t.CommissionId);
            
            CreateTable(
                "td.CjAdvertiserCommission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(),
                        ActionStatus = c.String(),
                        ActionTrackerId = c.String(),
                        ActionTrackerName = c.String(),
                        ActionType = c.String(),
                        AdvCommissionAmountUsd = c.Decimal(nullable: false, precision: 18, scale: 6),
                        AdvertiserId = c.String(),
                        AdvertiserName = c.String(),
                        Aid = c.String(),
                        CjFeeUsd = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ClickReferringUrl = c.String(),
                        CommissionId = c.String(),
                        ConcludingBrowser = c.String(),
                        ConcludingDeviceName = c.String(),
                        ConcludingDeviceType = c.String(),
                        Country = c.String(),
                        Coupon = c.String(),
                        EventDate = c.Int(nullable: false),
                        InitiatingBrowser = c.String(),
                        InitiatingDeviceName = c.String(),
                        InitiatingDeviceType = c.String(),
                        IsCrossDevice = c.Boolean(nullable: false),
                        NewToFile = c.Boolean(nullable: false),
                        OrderDiscountUsd = c.Decimal(nullable: false, precision: 18, scale: 6),
                        OrderId = c.String(),
                        Original = c.Boolean(nullable: false),
                        OriginalActionId = c.String(),
                        PostingDate = c.Int(nullable: false),
                        PublisherId = c.String(),
                        PublisherName = c.String(),
                        ReviewedStatus = c.String(),
                        SaleAmountUsd = c.Decimal(nullable: false, precision: 18, scale: 6),
                        SiteToStoreOffer = c.String(),
                        Source = c.String(),
                        WebsiteId = c.String(),
                        WebsiteName = c.String(),
                        CampaignId = c.String(),
                        CampaignName = c.String(),
                        City = c.String(),
                        CountryCode = c.String(),
                        ItemId = c.String(),
                        PaymentMethod = c.String(),
                        PlatformId = c.String(),
                        State = c.String(),
                        Upsell = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.CjAdvertiserCommissionItem", "CommissionId", "td.CjAdvertiserCommission");
            DropForeignKey("td.CjAdvertiserCommission", "AccountId", "td.Account");
            DropIndex("td.CjAdvertiserCommission", new[] { "AccountId" });
            DropIndex("td.CjAdvertiserCommissionItem", new[] { "CommissionId" });
            DropTable("td.CjAdvertiserCommission");
            DropTable("td.CjAdvertiserCommissionItem");
        }
    }
}
