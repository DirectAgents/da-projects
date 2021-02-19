namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewVcdMetrics : DbMigration
    {
        public override void Up()
        {
            AddColumn("td.VcdAnalytic", "SalesRank", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "AverageSalesPrice", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "SellableOnHandUnits", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "NumberOfCustomerReviews", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "NumberOfCustomerReviewsLifeToDate", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "AverageCustomerRating", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "FiveStars", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "FourStars", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "ThreeStars", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "TwoStars", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "OneStar", c => c.Decimal(precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            DropColumn("td.VcdAnalytic", "OneStar");
            DropColumn("td.VcdAnalytic", "TwoStars");
            DropColumn("td.VcdAnalytic", "ThreeStars");
            DropColumn("td.VcdAnalytic", "FourStars");
            DropColumn("td.VcdAnalytic", "FiveStars");
            DropColumn("td.VcdAnalytic", "AverageCustomerRating");
            DropColumn("td.VcdAnalytic", "NumberOfCustomerReviewsLifeToDate");
            DropColumn("td.VcdAnalytic", "NumberOfCustomerReviews");
            DropColumn("td.VcdAnalytic", "SellableOnHandUnits");
            DropColumn("td.VcdAnalytic", "AverageSalesPrice");
            DropColumn("td.VcdAnalytic", "SalesRank");
        }
    }
}
