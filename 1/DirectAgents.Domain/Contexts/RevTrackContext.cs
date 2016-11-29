using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.RevTrack;

namespace DirectAgents.Domain.Contexts
{
    public class RevTrackContext : DbContext
    {
        public const string rtSchema = "rt";
        public const string tblClientProg = "ClientProg";
        public const string tblVendorProg = "VendorProg";

        //? set CommandTimeout in constructor ?

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Client>().ToTable("Client", rtSchema);
            modelBuilder.Entity<ClientProg>().ToTable(tblClientProg, rtSchema);
            modelBuilder.Entity<Vendor>().ToTable("Vendor", rtSchema);
            modelBuilder.Entity<VendorProg>().ToTable(tblVendorProg, rtSchema);
            modelBuilder.Entity<ProgBudgetInfo>().ToTable("ProgBudgetInfo", rtSchema);
            modelBuilder.Entity<ProgVendorBudgetInfo>().ToTable("ProgVendorBudgetInfo", rtSchema);

            modelBuilder.Entity<ClientProg>().Property(c => c.DefaultBudgetInfo.MediaSpend).HasPrecision(14, 2).HasColumnName("MediaSpend");
            modelBuilder.Entity<ClientProg>().Property(c => c.DefaultBudgetInfo.MgmtFeePct).HasPrecision(10, 5).HasColumnName("MgmtFeePct");
            modelBuilder.Entity<ClientProg>().Property(c => c.DefaultBudgetInfo.MarginPct).HasPrecision(10, 5).HasColumnName("MarginPct");
            modelBuilder.Entity<ProgBudgetInfo>().Property(b => b.MediaSpend).HasPrecision(14, 2);
            modelBuilder.Entity<ProgBudgetInfo>().Property(b => b.MgmtFeePct).HasPrecision(10, 5);
            modelBuilder.Entity<ProgBudgetInfo>().Property(b => b.MarginPct).HasPrecision(10, 5);
            modelBuilder.Entity<ProgBudgetInfo>()
                .HasKey(b => new { b.ClientId, b.Date });
            modelBuilder.Entity<ProgVendorBudgetInfo>().Property(b => b.MediaSpend).HasPrecision(14, 2);
            modelBuilder.Entity<ProgVendorBudgetInfo>().Property(b => b.MgmtFeePct).HasPrecision(10, 5);
            modelBuilder.Entity<ProgVendorBudgetInfo>().Property(b => b.MarginPct).HasPrecision(10, 5);
            modelBuilder.Entity<ProgVendorBudgetInfo>()
                .HasKey(b => new { b.ClientId, b.VendorId, b.Date });
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientProg> ClientProgs { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorProg> VendorProgs { get; set; }
        public DbSet<ProgBudgetInfo> ProgBudgetInfos { get; set; }
        public DbSet<ProgVendorBudgetInfo> ProgVendorBudgetInfos { get; set; }

    }
}
