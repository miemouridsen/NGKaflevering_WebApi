using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class MeasurementDbContext : DbContext
    {
        public MeasurementDbContext(DbContextOptions<MeasurementDbContext> options)
            : base(options) { }

        public DbSet<Measurement> Measurements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Measurement>()
            //    .Property(m => m.Temperature)
            //    .HasPrecision(1);

            //modelBuilder.Entity<Measurement>()
            //    .Property(m => m.AirPressure)
            //    .HasPrecision(1);
        }
    }
}
