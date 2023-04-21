using Microsoft.EntityFrameworkCore;

namespace jenjennewborncare.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Review> review { get; set; }
        public DbSet<BabyCareServiceModel> BabyCareServices { get; set; }
        public DbSet<ServiceImage> ServiceImages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=jenjeare_main;uid=jenje_01;pwd=?a5F7ds71");
        }
    }


}