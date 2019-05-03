namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_EventConv_TransactionId : DbMigration
    {
        public override void Up()
        {
            AddColumn("cake.EventConversion", "TransactionId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("cake.EventConversion", "TransactionId");
        }
    }
}
