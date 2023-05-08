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

    public jenjennewborncareContext(DbContextOptions<jenjennewborncareContext> options)
        : base(options)
    {
    }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
