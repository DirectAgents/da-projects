namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_Affiliate_AccountManager : DbMigration
    {
        public override void Up()
        {
            AddColumn("cake.Affiliate", "AccountManagerId", c => c.Int());
            CreateIndex("cake.Affiliate", "AccountManagerId");
            AddForeignKey("cake.Affiliate", "AccountManagerId", "cake.Contact", "ContactId");
        }
        
        public override void Down()
        {
            DropForeignKey("cake.Affiliate", "AccountManagerId", "cake.Contact");
            DropIndex("cake.Affiliate", new[] { "AccountManagerId" });
            DropColumn("cake.Affiliate", "AccountManagerId");
        }
    }
}
