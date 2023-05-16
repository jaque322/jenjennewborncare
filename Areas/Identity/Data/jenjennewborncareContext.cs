using jenjennewborncare.Areas.Identity.Data;
using jenjennewborncare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace jenjennewborncare.Data;

public class jenjennewborncareContext : IdentityDbContext<User>
{

    public DbSet<Review> review { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<Nannie> Nannies { get; set; }
    public DbSet<ScheduleService> ScheduleServices { get; set; }
    public DbSet<Schedule> Schedules{ get; set; }
    public DbSet<Invoice> Invoices{ get; set; }

    public jenjennewborncareContext(DbContextOptions<jenjennewborncareContext> options)
        : base(options)
    {
    }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<ScheduleService>()
            .HasKey(ss => new { ss.ScheduleId, ss.ServiceId });

        builder.Entity<ScheduleService>()
            .HasOne(ss => ss.Schedule)
            .WithMany(s => s.ScheduleServices)
            .HasForeignKey(ss => ss.ScheduleId);

        builder.Entity<ScheduleService>()
            .HasOne(ss => ss.Service)
            .WithMany(s => s.ScheduleServices)
            .HasForeignKey(ss => ss.ServiceId);

        // If you have any other relationships to configure, add them here
    }

}
