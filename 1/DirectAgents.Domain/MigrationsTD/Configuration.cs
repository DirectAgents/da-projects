namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DirectAgents.Domain.Contexts.ClientPortalProgContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"MigrationsTD";
        }

        protected override void Seed(DirectAgents.Domain.Contexts.ClientPortalProgContext context)
        {
        }
    }
}
