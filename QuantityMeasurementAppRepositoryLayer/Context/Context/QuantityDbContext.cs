using Microsoft.EntityFrameworkCore;
using QuantityMeasurementAppModel.Entities;

namespace QuantityMeasurementAppRepositoryLayer.Context
{
    public class QuantityDbContext : DbContext
    {
        public QuantityDbContext(DbContextOptions<QuantityDbContext> options)
            : base(options)
        {
        }

        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                entity.ToTable("QuantityMeasurements");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Operation).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ThisUnit).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ThisMeasurementType).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ThatUnit).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ThatMeasurementType).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Operation).HasDatabaseName("IX_QM_Operation");
                entity.HasIndex(e => e.IsError).HasDatabaseName("IX_QM_IsError");
                entity.HasIndex(e => e.ThisMeasurementType).HasDatabaseName("IX_QM_MeasurementType");
                entity.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_QM_CreatedAt");
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Salt).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Role).HasMaxLength(50).HasDefaultValue("User");
                entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("IX_Users_Email");
                entity.HasIndex(e => e.Username).IsUnique().HasDatabaseName("IX_Users_Username");
            });
        }

        public override int SaveChanges()
        {
            SetTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetTimestamps()
        {
            var entries = ChangeTracker.Entries<QuantityMeasurementEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}