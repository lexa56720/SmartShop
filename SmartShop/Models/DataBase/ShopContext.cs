using Microsoft.EntityFrameworkCore;

namespace SmartShop.Models.DataBase
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Smartphone> Smartphones { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Producer> Producers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }


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
                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.Property(e => e.Token).HasColumnName("token");

                entity.HasOne(e => e.Role).WithOne()
                      .HasForeignKey<Role>(r=>r.Id);

                entity.HasMany(e => e.Orders).WithOne(o => o.User)
                      .HasForeignKey(o=>o.UserId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(e => e.Id).HasName("role_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Roles).HasColumnName("Roles");
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

                entity.HasOne(e => e.Producer).WithMany(p => p.Smartphones)
                      .HasForeignKey(s => s.ProducerId);
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.ToTable("producers");
                entity.HasKey(e => e.Id).HasName("producer_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("Name");
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


                entity.HasMany(e => e.Smartphones).WithMany();
            });


        }
    }
}
