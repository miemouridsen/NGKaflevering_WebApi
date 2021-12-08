using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Models
{
    public class MeasurementDbContext : DbContext, IMeasurementDbContext
    {
        public MeasurementDbContext(DbContextOptions<MeasurementDbContext> options)
            : base(options) { }

        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Location> Locations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
        public DbSet<WebApi.Models.User> User { get; set; }
    }


    public interface IMeasurementDbContext
    {
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<WebApi.Models.User> User { get; set; }
    }
}
