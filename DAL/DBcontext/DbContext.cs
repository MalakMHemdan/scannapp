using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Branches> Branches { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Points> Points { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserOffer> UserOffers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // =========================
            // Decimal precision fix
            // =========================
            builder.Entity<Order>()
                .Property(o => o.SubTotal).HasPrecision(18, 2);
            builder.Entity<Order>()
                .Property(o => o.Total_Amount).HasPrecision(18, 2);
            builder.Entity<Order>()
                .Property(o => o.Discount_Amount).HasPrecision(18, 2);

            builder.Entity<OrderItem>()
                .Property(oi => oi.Price).HasPrecision(18, 2);

            builder.Entity<Product>()
                .Property(p => p.Product_Price).HasPrecision(18, 2);

            builder.Entity<Offer>()
                .Property(o => o.Discount_Amount).HasPrecision(18, 2);
            builder.Entity<Offer>()
                .Property(o => o.Discount_percentage).HasPrecision(5, 2);

            // =========================
            // Admain ↔ Branch (Many-to-One)
            // =========================
            builder.Entity<Admain>()
                .HasOne<Branches>()
                .WithMany()
                .HasForeignKey(a => a.BranchID)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // Orders ↔ User (One-to-Many)
            // =========================
            builder.Entity<Order>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // OrderItems ↔ Order (One-to-Many)
            // =========================
            builder.Entity<OrderItem>()
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // Product ↔ Branch (Many-to-One)
            // =========================
            builder.Entity<Product>()
                .HasOne<Branches>()
                .WithMany()
                .HasForeignKey(p => p.BranchID)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // Points ↔ User, Admain, Branch
            // =========================
            builder.Entity<Points>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Points>()
                .HasOne<Admain>()
                .WithMany()
                .HasForeignKey(p => p.AdminID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Points>()
                .HasOne<Branches>()
                .WithMany()
                .HasForeignKey(p => p.BranchId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // UserOffer (Many-to-Many Users ↔ Offers)
            // =========================
            builder.Entity<UserOffer>()
        .HasKey(uo => new { uo.Userid, uo.offerId });

            builder.Entity<UserOffer>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(uo => uo.Userid)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<UserOffer>()
                .HasOne<Offer>()
                .WithMany()
                .HasForeignKey(uo => uo.offerId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<UserOffer>()
                .HasOne<SuperAdmain>()
                .WithMany()
                .HasForeignKey(uo => uo.SuperAdmainID)
                .OnDelete(DeleteBehavior.Restrict); // 

            // =========================
            // SuperAdmin ↔ Branches (One-to-Many)
            // =========================
            builder.Entity<SuperAdmain>()
                .HasMany<Branches>()
                .WithOne()
                .HasForeignKey(b => b.SuperAdmainID)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // Admain ↔ Branch (One-to-One)
            // =========================
            builder.Entity<Admain>()
                .HasOne<Branches>()
                .WithOne()
                .HasForeignKey<Admain>(a => a.BranchID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
