using Microsoft.EntityFrameworkCore;
using qma_service.Models;

namespace qma_service.Data
{
    public class QmaDbContext : DbContext
    {
        public QmaDbContext(DbContextOptions<QmaDbContext> options) : base(options) { }

        public DbSet<QuantityMeasurementEntity> Measurements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ThisUnit).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ThatUnit).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Operation).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}