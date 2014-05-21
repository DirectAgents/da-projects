namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WikiSchema : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.Campaigns", newSchema: "wiki");
            MoveTable(name: "dbo.Status", newSchema: "wiki");
            MoveTable(name: "dbo.Verticals", newSchema: "wiki");
            MoveTable(name: "dbo.People", newSchema: "wiki");
            MoveTable(name: "dbo.Countries", newSchema: "wiki");
            MoveTable(name: "dbo.TrafficTypes", newSchema: "wiki");
            MoveTable(name: "dbo.Conversions", newSchema: "wiki");
            MoveTable(name: "dbo.DailySummaries", newSchema: "wiki");
            MoveTable(name: "dbo.CampaignAccountManagers", newSchema: "wiki");
            MoveTable(name: "dbo.CampaignAdManagers", newSchema: "wiki");
            MoveTable(name: "dbo.CampaignCountries", newSchema: "wiki");
            MoveTable(name: "dbo.CampaignTrafficTypes", newSchema: "wiki");
        }
        
        public override void Down()
        {
            MoveTable(name: "wiki.CampaignTrafficTypes", newSchema: "dbo");
            MoveTable(name: "wiki.CampaignCountries", newSchema: "dbo");
            MoveTable(name: "wiki.CampaignAdManagers", newSchema: "dbo");
            MoveTable(name: "wiki.CampaignAccountManagers", newSchema: "dbo");
            MoveTable(name: "wiki.DailySummaries", newSchema: "dbo");
            MoveTable(name: "wiki.Conversions", newSchema: "dbo");
            MoveTable(name: "wiki.TrafficTypes", newSchema: "dbo");
            MoveTable(name: "wiki.Countries", newSchema: "dbo");
            MoveTable(name: "wiki.People", newSchema: "dbo");
            MoveTable(name: "wiki.Verticals", newSchema: "dbo");
            MoveTable(name: "wiki.Status", newSchema: "dbo");
            MoveTable(name: "wiki.Campaigns", newSchema: "dbo");
        }
    }
}
