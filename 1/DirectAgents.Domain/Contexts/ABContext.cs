using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Domain.Contexts
{
    public class ABContext : DbContext
    {
        public const string abSchema = "ab";

        //? set CommandTimeout in constructor ?

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // --- AB ---
            modelBuilder.Entity<ABClient>().ToTable("Client", abSchema);
        }

        // --- AB ---
        public DbSet<ABClient> ABClients { get; set; }
    }
}
