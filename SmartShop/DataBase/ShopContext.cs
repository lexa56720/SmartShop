using Microsoft.EntityFrameworkCore;
using SmartShop.DataBase.Tables;

namespace SmartShop.DataBase
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
            
          //  Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Smartphone> Smartphones { get; set; }

        public virtual DbSet<Producer> Producers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Media> Medias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<User>(entity =>
             {
                 entity.ToTable("users");
                 entity.HasKey(e => e.Id).HasName("user_pkey");
                 entity.Property(e => e.Id).HasColumnName("id");
                 entity.Property(e => e.Login).HasColumnName("login");
                 entity.Property(e => e.Password).HasColumnName("password");
                 entity.Property(e => e.Name).HasColumnName("name");
                 entity.Property(e => e.Token).HasColumnName("token");
                 entity.Property(e => e.Role).HasColumnName("role");


                 entity.HasMany(e => e.Orders).WithOne(o => o.User)
                       .HasForeignKey(o => o.UserId);
             });



            modelBuilder.Entity<Smartphone>(entity =>
            {
                entity.ToTable("smartphones");
                entity.HasKey(e => e.Id).HasName("smartphone_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.MegaPixels).HasColumnName("megapixels");
                entity.Property(e => e.MemorySize).HasColumnName("memory_size");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.ProducerId).HasColumnName("producer_id");
                entity.Property(e => e.RamSize).HasColumnName("ram_size");
                entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.UnitsAvailable).HasColumnName("units_available");

                entity.HasOne(e => e.Producer).WithMany(p => p.Smartphones)
                      .HasForeignKey(s => s.ProducerId);
            });
            modelBuilder.Entity<Media>(entity =>
            {
                entity.ToTable("medias");
                entity.HasKey(e => e.Id).HasName("media_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Url).HasColumnName("url");
                entity.Property(e => e.SmartphoneId).HasColumnName("smartphone_id");
                entity.Property(e => e.Data).HasColumnName("data");


                entity.HasOne(e => e.Smartphone).WithMany(s => s.Medias)
                      .HasForeignKey(e => e.SmartphoneId);
            });
            modelBuilder.Entity<Producer>(entity =>
            {
                entity.ToTable("producers");
                entity.HasKey(e => e.Id).HasName("producer_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.HasKey(e => e.Id).HasName("order_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.OrderDate).HasColumnName("order_date");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Code).HasColumnName("code");

                entity.HasMany(e => e.Smartphones).WithMany();
            });


        }
    }
}
