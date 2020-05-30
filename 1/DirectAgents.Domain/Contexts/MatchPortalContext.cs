using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

using DirectAgents.Domain.Entities.MatchPortal;

namespace DirectAgents.Domain.Contexts
{
    public class MatchPortalContext : DbContext
    {
        private const string dboScheme = "dbo";

        public MatchPortalContext()
            : base("name=DAData")
        {
            var adapter = (IObjectContextAdapter)this;
            var objectContext = adapter.ObjectContext;
            objectContext.CommandTimeout = 5 * 60;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<MatchingProduct>().ToTable("buyma_source", dboScheme);
            modelBuilder.Entity<MatchingResult>().ToTable("buyma_matching_result", dboScheme);
        }

        public DbSet<MatchingResult> MatchingResults { get; set; }

        public DbSet<MatchingProduct> MatchingProducts { get; set; }
    }
}
