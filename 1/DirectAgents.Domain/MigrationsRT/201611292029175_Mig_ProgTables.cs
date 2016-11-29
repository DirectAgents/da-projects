namespace DirectAgents.Domain.MigrationsRT
{
    using System;
    using System.Data.Entity.Migrations;
    using DirectAgents.Domain.Contexts;

    public partial class Mig_ProgTables : DbMigration
    {
        private const string indexCode = "IX_UQ_Code";
        private const string tableClientProg = RevTrackContext.rtSchema + "." + RevTrackContext.tblClientProg;
        private const string tableVendorProg = RevTrackContext.rtSchema + "." + RevTrackContext.tblVendorProg;
        private const string columnCode = "Code";

        public override void Up()
        {
            CreateTable(
                "rt.ClientProg",
                c => new
                    {
                        ClientId = c.Int(nullable: false),
                        Code = c.String(maxLength: 50),
                        MediaSpend = c.Decimal(nullable: false, precision: 14, scale: 2),
                        MgmtFeePct = c.Decimal(nullable: false, precision: 10, scale: 5),
                        MarginPct = c.Decimal(nullable: false, precision: 10, scale: 5),
                    })
                .PrimaryKey(t => t.ClientId)
                .ForeignKey("rt.Client", t => t.ClientId)
                .Index(t => t.ClientId);
            Sql(string.Format(@"
                CREATE UNIQUE NONCLUSTERED INDEX {0}
                ON {1}({2})
                WHERE {2} IS NOT NULL;",
                indexCode, tableClientProg, columnCode));

            CreateTable(
                "rt.ProgBudgetInfo",
                c => new
                    {
                        ClientId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        MediaSpend = c.Decimal(nullable: false, precision: 14, scale: 2),
                        MgmtFeePct = c.Decimal(nullable: false, precision: 10, scale: 5),
                        MarginPct = c.Decimal(nullable: false, precision: 10, scale: 5),
                    })
                .PrimaryKey(t => new { t.ClientId, t.Date })
                .ForeignKey("rt.Client", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("rt.ClientProg", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);

            CreateTable(
                "rt.ProgVendorBudgetInfo",
                c => new
                    {
                        ClientId = c.Int(nullable: false),
                        VendorId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        MediaSpend = c.Decimal(nullable: false, precision: 14, scale: 2),
                        MgmtFeePct = c.Decimal(nullable: false, precision: 10, scale: 5),
                        MarginPct = c.Decimal(nullable: false, precision: 10, scale: 5),
                    })
                .PrimaryKey(t => new { t.ClientId, t.VendorId, t.Date })
                .ForeignKey("rt.Client", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("rt.Vendor", t => t.VendorId, cascadeDelete: true)
                .ForeignKey("rt.ClientProg", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.VendorId);
            
            CreateTable(
                "rt.VendorProg",
                c => new
                    {
                        VendorId = c.Int(nullable: false),
                        Code = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.VendorId)
                .ForeignKey("rt.Vendor", t => t.VendorId)
                .Index(t => t.VendorId);
            Sql(string.Format(@"
                CREATE UNIQUE NONCLUSTERED INDEX {0}
                ON {1}({2})
                WHERE {2} IS NOT NULL;",
                indexCode, tableVendorProg, columnCode));
        }
        
        public override void Down()
        {
            DropForeignKey("rt.ProgVendorBudgetInfo", "ClientId", "rt.ClientProg");
            DropForeignKey("rt.ProgVendorBudgetInfo", "VendorId", "rt.Vendor");
            DropForeignKey("rt.VendorProg", "VendorId", "rt.Vendor");
            DropForeignKey("rt.ProgVendorBudgetInfo", "ClientId", "rt.Client");
            DropForeignKey("rt.ClientProg", "ClientId", "rt.Client");
            DropForeignKey("rt.ProgBudgetInfo", "ClientId", "rt.ClientProg");
            DropForeignKey("rt.ProgBudgetInfo", "ClientId", "rt.Client");
            DropIndex("rt.VendorProg", new[] { "VendorId" });
            DropIndex("rt.ProgVendorBudgetInfo", new[] { "VendorId" });
            DropIndex("rt.ProgVendorBudgetInfo", new[] { "ClientId" });
            DropIndex("rt.ProgBudgetInfo", new[] { "ClientId" });
            DropIndex("rt.ClientProg", new[] { "ClientId" });
            DropTable("rt.VendorProg");
            DropTable("rt.ProgVendorBudgetInfo");
            DropTable("rt.ProgBudgetInfo");
            DropTable("rt.ClientProg");
        }
    }
}
