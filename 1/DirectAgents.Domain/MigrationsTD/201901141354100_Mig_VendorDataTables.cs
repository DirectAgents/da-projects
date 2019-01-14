namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_VendorDataTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.VCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.VCategorySummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.CategoryId, t.MetricTypeId })
                .ForeignKey("td.VCategory", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.MetricTypeId);
            
            CreateTable(
                "td.VProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asin = c.String(),
                        CategoryId = c.Int(),
                        SubcategoryId = c.Int(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.VCategory", t => t.CategoryId)
                .ForeignKey("td.Account", t => t.AccountId)
                .ForeignKey("td.VSubcategory", t => t.SubcategoryId)
                .Index(t => t.CategoryId)
                .Index(t => t.SubcategoryId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.VSubcategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.VCategory", t => t.CategoryId)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.CategoryId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.VProductSummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        ProductId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.ProductId, t.MetricTypeId })
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .ForeignKey("td.VProduct", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.MetricTypeId);
            
            CreateTable(
                "td.VSubcategorySummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        SubcategoryId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.SubcategoryId, t.MetricTypeId })
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .ForeignKey("td.VSubcategory", t => t.SubcategoryId, cascadeDelete: true)
                .Index(t => t.SubcategoryId)
                .Index(t => t.MetricTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.VSubcategorySummaryMetric", "SubcategoryId", "td.VSubcategory");
            DropForeignKey("td.VSubcategorySummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.VProductSummaryMetric", "ProductId", "td.VProduct");
            DropForeignKey("td.VProductSummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.VProduct", "SubcategoryId", "td.VSubcategory");
            DropForeignKey("td.VSubcategory", "AccountId", "td.Account");
            DropForeignKey("td.VSubcategory", "CategoryId", "td.VCategory");
            DropForeignKey("td.VProduct", "AccountId", "td.Account");
            DropForeignKey("td.VProduct", "CategoryId", "td.VCategory");
            DropForeignKey("td.VCategorySummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.VCategorySummaryMetric", "CategoryId", "td.VCategory");
            DropForeignKey("td.VCategory", "AccountId", "td.Account");
            DropIndex("td.VSubcategorySummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.VSubcategorySummaryMetric", new[] { "SubcategoryId" });
            DropIndex("td.VProductSummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.VProductSummaryMetric", new[] { "ProductId" });
            DropIndex("td.VSubcategory", new[] { "AccountId" });
            DropIndex("td.VSubcategory", new[] { "CategoryId" });
            DropIndex("td.VProduct", new[] { "AccountId" });
            DropIndex("td.VProduct", new[] { "SubcategoryId" });
            DropIndex("td.VProduct", new[] { "CategoryId" });
            DropIndex("td.VCategorySummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.VCategorySummaryMetric", new[] { "CategoryId" });
            DropIndex("td.VCategory", new[] { "AccountId" });
            DropTable("td.VSubcategorySummaryMetric");
            DropTable("td.VProductSummaryMetric");
            DropTable("td.VSubcategory");
            DropTable("td.VProduct");
            DropTable("td.VCategorySummaryMetric");
            DropTable("td.VCategory");
        }
    }
}
