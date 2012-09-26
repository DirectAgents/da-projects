using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace ApiClient.Models.DirectTrack
{
    public class DirectTrackDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public DbSet<DirectTrackResource> DirectTrackResources { get; set; }
    }
}
