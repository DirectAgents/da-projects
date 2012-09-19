namespace ApiClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Conversion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.conversions",
                c => new
                    {
                        conversion_id = c.Int(nullable: false, identity: true),
                        visitor_id = c.Int(nullable: false),
                        request_session_id = c.Int(nullable: false),
                        click_id = c.Int(nullable: false),
                        conversion_date = c.DateTime(nullable: false),
                        affiliate_affiliate_id = c.Int(nullable: false),
                        affiliate_affiliate_name = c.String(),
                        advertiser_advertiser_id = c.Int(nullable: false),
                        advertiser_advertiser_name = c.String(),
                        offer_offer_id = c.Int(nullable: false),
                        offer_offer_name = c.String(),
                        creative_creative_id = c.Int(nullable: false),
                        creative_creative_name = c.String(),
                        sub_id_1 = c.String(),
                        sub_id_2 = c.String(),
                        sub_id_3 = c.String(),
                        conversion_type = c.String(),
                        paid_currency_id = c.Int(nullable: false),
                        paid_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        paid_formatted_amount = c.String(),
                        received_currency_id = c.Int(nullable: false),
                        received_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        received_formatted_amount = c.String(),
                        pixel_dropped = c.Boolean(nullable: false),
                        suppressed = c.Boolean(nullable: false),
                        returned = c.Boolean(nullable: false),
                        test = c.Boolean(nullable: false),
                        transaction_id = c.String(),
                        ip_address = c.String(),
                        referrer_url = c.String(),
                        note = c.String(),
                    })
                .PrimaryKey(t => t.conversion_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.conversions");
        }
    }
}
