namespace DirectAgents.Domain.MigrationsRT
{
    using System;
    using System.Data.Entity.Migrations;
    using DirectAgents.Domain.Contexts;
    
    public partial class Mig_ProgCampaign : DbMigration
    {
        private const string indexCode = "IX_UQ_Code";
        private const string tableClient = RevTrackContext.rtSchema + ".Client";
        private const string tableVendor = RevTrackContext.rtSchema + ".Vendor";
        private const string columnCode = "Code";

        public override void Up()
        {
            CreateTable(
                "rt.ProgCampaign",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        Name = c.String(),
                        Code = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("rt.Client", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            AddColumn("rt.Client", "Code", c => c.String(maxLength: 50));
            AddColumn("rt.Vendor", "Code", c => c.String(maxLength: 50));

            Sql(string.Format(@"
                CREATE UNIQUE NONCLUSTERED INDEX {0}
                ON {1}({2})
                WHERE {2} IS NOT NULL;",
                indexCode, tableClient, columnCode));
            Sql(string.Format(@"
                CREATE UNIQUE NONCLUSTERED INDEX {0}
                ON {1}({2})
                WHERE {2} IS NOT NULL;",
                indexCode, tableVendor, columnCode));
        }
        
        public override void Down()
        {
            DropForeignKey("rt.ProgCampaign", "ClientId", "rt.Client");
            DropIndex("rt.ProgCampaign", new[] { "ClientId" });
            DropIndex(tableVendor, indexCode);
            DropIndex(tableClient, indexCode);
            DropColumn("rt.Vendor", "Code");
            DropColumn("rt.Client", "Code");
            DropTable("rt.ProgCampaign");
        }
    }
}
