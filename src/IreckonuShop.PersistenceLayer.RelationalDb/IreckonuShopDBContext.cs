using IreckonuShop.PersistenceLayer.RelationalDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace IreckonuShop.PersistenceLayer.RelationalDb
{
    public class IreckonuShopDbContext : DbContext 
    {
        public IreckonuShopDbContext(DbContextOptions options)
        : base(options)
        {
        }

        public DbSet<ProductEntity> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEntity>().ToTable("Products").HasKey(p => p.Id);
            modelBuilder.Entity<ProductEntity>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProductEntity>().HasAlternateKey(p => p.Key);
            modelBuilder.Entity<ProductEntity>().Property(p => p.Price).IsRequired();
            modelBuilder.Entity<ProductEntity>().Property(p => p.Discount).IsRequired();
            modelBuilder.Entity<ProductEntity>().Property(p => p.Description).IsRequired();
            modelBuilder.Entity<ProductEntity>().Property(p => p.Size).IsRequired();
            modelBuilder.Entity<ProductEntity>().HasOne(p => p.Type);
            modelBuilder.Entity<ProductEntity>().HasOne(p => p.Color);
            modelBuilder.Entity<ProductEntity>().HasOne(p => p.Delivery);
            modelBuilder.Entity<ProductEntity>().HasOne(p => p.TargetClient);

            modelBuilder.Entity<ProductTypeEntity>().ToTable("ProductType").HasKey(p => p.Id);
            modelBuilder.Entity<ProductTypeEntity>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProductTypeEntity>().HasAlternateKey(p => p.ArtikelCode);
            modelBuilder.Entity<ProductTypeEntity>().HasAlternateKey(p => p.ColorCode);

            modelBuilder.Entity<ColorEntity>().ToTable("Colors").HasKey(p => p.Argb);
            modelBuilder.Entity<ColorEntity>().Property(p => p.Argb).ValueGeneratedNever();
            modelBuilder.Entity<ColorEntity>().Property(p => p.Alpha).IsRequired();
            modelBuilder.Entity<ColorEntity>().Property(p => p.Red).IsRequired();
            modelBuilder.Entity<ColorEntity>().Property(p => p.Green).IsRequired();
            modelBuilder.Entity<ColorEntity>().Property(p => p.Blue).IsRequired();

            modelBuilder.Entity<TargetClientEntity>().ToTable("TargetClients").HasKey(p => p.Id);
            modelBuilder.Entity<TargetClientEntity>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TargetClientEntity>().HasAlternateKey(p => p.TargetClient);

            modelBuilder.Entity<DeliveryRangeEntity>().ToTable("DeliveryRanges").HasKey(p => p.Id);
            modelBuilder.Entity<DeliveryRangeEntity>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<DeliveryRangeEntity>().Property(p => p.Min).IsRequired();
            modelBuilder.Entity<DeliveryRangeEntity>().Property(p => p.Max).IsRequired();
        }
    }
}