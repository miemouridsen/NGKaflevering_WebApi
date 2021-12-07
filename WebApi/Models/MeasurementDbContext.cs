using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Models
{
    public class MeasurementDbContext : DbContext
    {
        public MeasurementDbContext(DbContextOptions<MeasurementDbContext> options)
            : base(options) { }

        public DbSet<Measurement> Measurements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
        public DbSet<WebApi.Models.User> User { get; set; }
    }
}
