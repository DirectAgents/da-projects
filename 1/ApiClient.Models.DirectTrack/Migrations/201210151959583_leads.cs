namespace ApiClient.Models.DirectTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class leads : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.leadDetails",
                c => new
                    {
                        location = c.String(nullable: false, maxLength: 500),
                        cookieID = c.String(),
                        affiliateResourceURL_location = c.String(),
                        campaignResourceURL_location = c.String(),
                        creativeResourceURL_location = c.String(),
                        creativeDeploymentResourceURL_location = c.String(),
                        date = c.String(),
                        subIDs_subID1 = c.String(),
                        subIDs_subID2 = c.String(),
                        subIDs_subID3 = c.String(),
                        subIDs_subID4 = c.String(),
                        subIDs_subID5 = c.String(),
                        ipAddress = c.String(),
                        refererURL = c.String(),
                        affOptInfo = c.String(),
                        advOptInfo = c.String(),
                        landingPageIDSpecified = c.Boolean(nullable: false),
                        poolIDSpecified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.location);
            
            CreateTable(
                "dbo.programLeads",
                c => new
                    {
                        location = c.String(nullable: false, maxLength: 500),
                        cookieID = c.String(),
                        affiliateResourceURL_location = c.String(),
                        campaignResourceURL_location = c.String(),
                        creativeResourceURL_location = c.String(),
                        creativeDeploymentResourceURL_location = c.String(),
                        date = c.String(),
                        subIDs_subID1 = c.String(),
                        subIDs_subID2 = c.String(),
                        subIDs_subID3 = c.String(),
                        subIDs_subID4 = c.String(),
                        subIDs_subID5 = c.String(),
                        landingPageIDSpecified = c.Boolean(nullable: false),
                        poolIDSpecified = c.Boolean(nullable: false),
                        theyGet = c.String(),
                        weGet = c.String(),
                    })
                .PrimaryKey(t => t.location);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.programLeads");
            DropTable("dbo.leadDetails");
        }
    }
}
