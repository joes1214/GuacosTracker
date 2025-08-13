using GuacosTracker3.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GuacosTracker3.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuacosTracker3.Data
{
    public class TrackerDbContext: IdentityDbContext<ApplicationUser>
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

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Auth0Id)
                .IsUnique();
            //builder.ApplyConfiguration(new ApplicationUserConfiguration());
        }

    }

    //public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    //{
    //    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    //    {
    //        builder.Property(u => u.FName).HasMaxLength(100);
    //        builder.Property(u => u.LName).HasMaxLength(100);

    //    }
    //}
}
