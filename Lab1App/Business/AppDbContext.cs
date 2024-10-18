using Business.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Security.Principal;

namespace Business
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasIndex(p => p.Name).IsUnique();

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(p => p.Description)
                    .HasMaxLength(500);

                entity.Property(p => p.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18, 2)");

                entity.Property(p => p.Stock)
                    .IsRequired()
                    .HasDefaultValue(0);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(l => l.LogMessage)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(l => l.LogTimeUTC)
                    .IsRequired();
            });
        }
    }
}
