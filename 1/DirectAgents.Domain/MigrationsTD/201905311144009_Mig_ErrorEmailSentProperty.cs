namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Mig_ErrorEmailSentProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("adm.JobRequestExecution", "ErrorEmailSent", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("adm.JobRequestExecution", "ErrorEmailSent");
        }
    }
}
