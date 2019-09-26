namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Contexts.ClientPortalProgContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"MigrationsTD";
            CommandTimeout = 60 * 5;
        }

        protected override void Seed(Contexts.ClientPortalProgContext context)
        {
        }
    }
}
