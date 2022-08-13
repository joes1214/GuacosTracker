using GuacosTracker3.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GuacosTracker3.Data
{
    public class TrackerDbContext: IdentityDbContext
    {
        public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Notes> Notes { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Ticket>().HasMany(m => m.CustomerId);

        //}

        
    }
}
