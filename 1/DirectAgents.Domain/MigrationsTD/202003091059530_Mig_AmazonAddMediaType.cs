namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;

    public partial class Mig_AmazonAddMediaType : DbMigration
    {
        public override void Up()
        {
            AddColumn("td.Keyword", "MediaType", c => c.String());
        }

        public override void Down()
        {
            DropColumn("td.Keyword", "MediaType");
        }
    }
}
