namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;
    
    public partial class Mig_ExecutionHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "adm.JobExecutionHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CommandName = c.String(),
                        ExecutionArguments = c.String(),
                        Status = c.String(),
                        CurrentState = c.String(),
                        Warnings = c.String(),
                        Errors = c.String(),
                        LaunchMessage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("adm.JobExecutionHistory");
        }
    }
}
