namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_AdRoll : DbMigration
    {
        // *** Moved to TDContext ***

        public override void Up()
        {
            //CreateTable(
            //    "adr.Advertisable",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            Eid = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            //DropTable("adr.Advertisable");
        }
    }
}
