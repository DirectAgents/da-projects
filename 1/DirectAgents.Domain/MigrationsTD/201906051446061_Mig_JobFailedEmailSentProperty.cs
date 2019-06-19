namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;

    public partial class Mig_JobFailedEmailSentProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("adm.JobRequest", "FailureEmailSent", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("adm.JobRequest", "FailureEmailSent");
        }
    }
}
