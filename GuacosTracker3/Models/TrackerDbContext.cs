using GuacosTracker3.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GuacosTracker3.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuacosTracker3.Data
{
    public class TrackerDbContext: IdentityDbContext
    {
        public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserConfiguration());
        }

    }

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<TrackerUser>
    {
        public void Configure(EntityTypeBuilder<TrackerUser> builder)
        {
            builder.Property(u => u.FName).HasMaxLength(100);
            builder.Property(u => u.LName).HasMaxLength(100);

        }
    }
}
