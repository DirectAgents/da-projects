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
            modelBuilder.Entity<ClientAccount>().ToTable("ClientAccount", abSchema);
            modelBuilder.Entity<AccountBudget>().ToTable("AccountBudget", abSchema)
                .HasKey(x => new { x.ClientAccountId, x.Date });

            modelBuilder.Entity<AccountBudget>().Property(x => x.Value).HasPrecision(14, 2);
        }

        // --- AB ---
        public DbSet<ABClient> ABClients { get; set; }
        public DbSet<ClientAccount> ClientAccounts { get; set; }
        public DbSet<AccountBudget> AccountBudgets { get; set; }
    }
}
