using System.Data.Entity;
using ClientPortal.Web.Models.Cake;

namespace CakeExtracter
{
    // HACK: this is in two places right now because referencing the web project causes a runtime error..
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<conversion> Conversions { get; set; }
        public DbSet<click> Clicks { get; set; }
    }
}