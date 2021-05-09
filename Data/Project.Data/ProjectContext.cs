using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Common;
using Project.Data.Models;

namespace Project.Data
{
    public class ProjectContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<RentedCars> RentedCars { get; set; }
        public DbSet<CarStatus> CarStatuses { get; set; }

        public ProjectContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Car>()
                .HasOne(x => x.CarStatus);

            base.OnModelCreating(builder);
        }
    }
}
