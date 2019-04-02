namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_JobExecution : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "adm.JobRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentJobRequestId = c.Int(),
                        CommandName = c.String(),
                        CommandExecutionArguments = c.String(),
                        ScheduledTime = c.DateTime(),
                        AttemptNumber = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("adm.JobRequest", t => t.ParentJobRequestId)
                .Index(t => t.ParentJobRequestId);
            
            CreateTable(
                "adm.JobRequestExecution",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobRequestId = c.Int(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Errors = c.String(),
                        Warnings = c.String(),
                        CurrentState = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("adm.JobRequest", t => t.JobRequestId)
                .Index(t => t.JobRequestId);
            
            DropTable("adm.JobExecutionHistory");
        }
        
        public override void Down()
        {
            CreateTable(
                "adm.JobExecutionHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        CommandName = c.String(),
                        ExecutionArguments = c.String(),
                        Status = c.String(),
                        CurrentState = c.String(),
                        Warnings = c.String(),
                        Errors = c.String(),
                        LaunchData = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("adm.JobRequestExecution", "JobRequestId", "adm.JobRequest");
            DropForeignKey("adm.JobRequest", "ParentJobRequestId", "adm.JobRequest");
            DropIndex("adm.JobRequestExecution", new[] { "JobRequestId" });
            DropIndex("adm.JobRequest", new[] { "ParentJobRequestId" });
            DropTable("adm.JobRequestExecution");
            DropTable("adm.JobRequest");
        }
    }
}
