namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_VendorDataTables_2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.VBrand",
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
                "td.VBrandSummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        BrandId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.BrandId, t.MetricTypeId })
                .ForeignKey("td.VBrand", t => t.BrandId, cascadeDelete: true)
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .Index(t => t.BrandId)
                .Index(t => t.MetricTypeId);
            
            CreateTable(
                "td.VCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BrandId = c.Int(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.VBrand", t => t.BrandId)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.BrandId)
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
                "td.VParentProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asin = c.String(),
                        CategoryId = c.Int(),
                        SubcategoryId = c.Int(),
                        BrandId = c.Int(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.VBrand", t => t.BrandId)
                .ForeignKey("td.VCategory", t => t.CategoryId)
                .ForeignKey("td.Account", t => t.AccountId)
                .ForeignKey("td.VSubcategory", t => t.SubcategoryId)
                .Index(t => t.CategoryId)
                .Index(t => t.SubcategoryId)
                .Index(t => t.BrandId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.VSubcategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(),
                        BrandId = c.Int(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.VBrand", t => t.BrandId)
                .ForeignKey("td.VCategory", t => t.CategoryId)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.CategoryId)
                .Index(t => t.BrandId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.VParentProductSummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        ParentProductId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.ParentProductId, t.MetricTypeId })
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .ForeignKey("td.VParentProduct", t => t.ParentProductId, cascadeDelete: true)
                .Index(t => t.ParentProductId)
                .Index(t => t.MetricTypeId);
            
            CreateTable(
                "td.VProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asin = c.String(),
                        CategoryId = c.Int(),
                        SubcategoryId = c.Int(),
                        ParentProductId = c.Int(),
                        BrandId = c.Int(),
                        Ean = c.String(),
                        Upc = c.String(),
                        ApparelSize = c.String(),
                        ApparelSizeWidth = c.String(),
                        Binding = c.String(),
                        Color = c.String(),
                        ModelStyleNumber = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.VBrand", t => t.BrandId)
                .ForeignKey("td.VCategory", t => t.CategoryId)
                .ForeignKey("td.Account", t => t.AccountId)
                .ForeignKey("td.VParentProduct", t => t.ParentProductId)
                .ForeignKey("td.VSubcategory", t => t.SubcategoryId)
                .Index(t => t.CategoryId)
                .Index(t => t.SubcategoryId)
                .Index(t => t.ParentProductId)
                .Index(t => t.BrandId)
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
            DropForeignKey("td.VProduct", "ParentProductId", "td.VParentProduct");
            DropForeignKey("td.VProduct", "AccountId", "td.Account");
            DropForeignKey("td.VProduct", "CategoryId", "td.VCategory");
            DropForeignKey("td.VProduct", "BrandId", "td.VBrand");
            DropForeignKey("td.VParentProductSummaryMetric", "ParentProductId", "td.VParentProduct");
            DropForeignKey("td.VParentProductSummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.VParentProduct", "SubcategoryId", "td.VSubcategory");
            DropForeignKey("td.VSubcategory", "AccountId", "td.Account");
            DropForeignKey("td.VSubcategory", "CategoryId", "td.VCategory");
            DropForeignKey("td.VSubcategory", "BrandId", "td.VBrand");
            DropForeignKey("td.VParentProduct", "AccountId", "td.Account");
            DropForeignKey("td.VParentProduct", "CategoryId", "td.VCategory");
            DropForeignKey("td.VParentProduct", "BrandId", "td.VBrand");
            DropForeignKey("td.VCategorySummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.VCategorySummaryMetric", "CategoryId", "td.VCategory");
            DropForeignKey("td.VCategory", "AccountId", "td.Account");
            DropForeignKey("td.VCategory", "BrandId", "td.VBrand");
            DropForeignKey("td.VBrandSummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.VBrandSummaryMetric", "BrandId", "td.VBrand");
            DropForeignKey("td.VBrand", "AccountId", "td.Account");
            DropIndex("td.VSubcategorySummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.VSubcategorySummaryMetric", new[] { "SubcategoryId" });
            DropIndex("td.VProductSummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.VProductSummaryMetric", new[] { "ProductId" });
            DropIndex("td.VProduct", new[] { "AccountId" });
            DropIndex("td.VProduct", new[] { "BrandId" });
            DropIndex("td.VProduct", new[] { "ParentProductId" });
            DropIndex("td.VProduct", new[] { "SubcategoryId" });
            DropIndex("td.VProduct", new[] { "CategoryId" });
            DropIndex("td.VParentProductSummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.VParentProductSummaryMetric", new[] { "ParentProductId" });
            DropIndex("td.VSubcategory", new[] { "AccountId" });
            DropIndex("td.VSubcategory", new[] { "BrandId" });
            DropIndex("td.VSubcategory", new[] { "CategoryId" });
            DropIndex("td.VParentProduct", new[] { "AccountId" });
            DropIndex("td.VParentProduct", new[] { "BrandId" });
            DropIndex("td.VParentProduct", new[] { "SubcategoryId" });
            DropIndex("td.VParentProduct", new[] { "CategoryId" });
            DropIndex("td.VCategorySummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.VCategorySummaryMetric", new[] { "CategoryId" });
            DropIndex("td.VCategory", new[] { "AccountId" });
            DropIndex("td.VCategory", new[] { "BrandId" });
            DropIndex("td.VBrandSummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.VBrandSummaryMetric", new[] { "BrandId" });
            DropIndex("td.VBrand", new[] { "AccountId" });
            DropTable("td.VSubcategorySummaryMetric");
            DropTable("td.VProductSummaryMetric");
            DropTable("td.VProduct");
            DropTable("td.VParentProductSummaryMetric");
            DropTable("td.VSubcategory");
            DropTable("td.VParentProduct");
            DropTable("td.VCategorySummaryMetric");
            DropTable("td.VCategory");
            DropTable("td.VBrandSummaryMetric");
            DropTable("td.VBrand");
        }
    }
}
