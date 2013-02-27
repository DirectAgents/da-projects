using System.Data.Entity;

namespace Huggies.Web.Models
{
    public class Context : DbContext
    {
        public DbSet<Lead> Leads { get; set; }
    }
}