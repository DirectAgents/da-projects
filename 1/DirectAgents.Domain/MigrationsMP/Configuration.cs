using System.Data.Entity.Migrations;

namespace DirectAgents.Domain.MigrationsMP
{
    internal sealed class Configuration : DbMigrationsConfiguration<DirectAgents.Domain.Contexts.MatchPortalContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"MigrationsMP";
        }

        protected override void Seed(DirectAgents.Domain.Contexts.MatchPortalContext context)
        {
        }
    }
}
