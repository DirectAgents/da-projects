namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;

    public partial class Mig_FacebookAddSeparatedReachMetric : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.FbReachMetric",
                c => new
                    {
                        AccountId = c.Int(nullable: false),
                        Period = c.String(nullable: false, maxLength: 128),
                        Reach = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccountId, t.Period })
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);

            DropColumn("td.FbAdSetSummary", "Reach");
            DropColumn("td.FbAdSummary", "Reach");
            DropColumn("td.FbCampaignSummary", "Reach");
            DropColumn("td.FbDailySummary", "Reach");
        }

        public override void Down()
        {
            DropForeignKey("td.FbReachMetric", "AccountId", "td.Account");
            DropIndex("td.FbReachMetric", new[] { "AccountId" });
            DropTable("td.FbReachMetric");
        }
    }
}
