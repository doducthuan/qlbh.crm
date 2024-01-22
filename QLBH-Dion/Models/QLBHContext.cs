using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using QLBH_Dion.Util;

namespace QLBH_Dion.Models
{
    public partial class QLBHContext : DbContext
    {
        public QLBHContext()
        {
        }

        public QLBHContext(DbContextOptions<QLBHContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<AccountBuy> AccountBuys { get; set; } = null!;
        public virtual DbSet<AccountStatus> AccountStatuses { get; set; } = null!;
        public virtual DbSet<ActivityLog> ActivityLogs { get; set; } = null!;
        public virtual DbSet<AuctionProduct> AuctionProducts { get; set; } = null!;
        public virtual DbSet<AuctionProductStatus> AuctionProductStatuses { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<NotificationStatus> NotificationStatuses { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderAccount> OrderAccounts { get; set; } = null!;
        public virtual DbSet<OrderAccountPaymentStatus> OrderAccountPaymentStatuses { get; set; } = null!;
        public virtual DbSet<OrderAccountStatus> OrderAccountStatuses { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<OrderType> OrderTypes { get; set; } = null!;
        public virtual DbSet<OrderUpdateHistory> OrderUpdateHistories { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductBrand> ProductBrands { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public virtual DbSet<Province> Provinces { get; set; } = null!;
        public virtual DbSet<Right> Rights { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RoleRight> RoleRights { get; set; } = null!;
        public virtual DbSet<Setting> Settings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=103.214.9.237;Database=QLBH;UID=dionsa;PWD=Z4dbcDkIkNxAJ4H;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Cách sử dụng CustomQuery
            //1.Thêm vào DB context
            modelBuilder.HasDbFunction(typeof(CustomQuery).GetMethod(nameof(CustomQuery.ToCustomString))).HasTranslation(
                 e =>
                 {
                     return new SqlFunctionExpression(functionName: "format", arguments: new[]{
                                 e.First(),
                                 new SqlFragmentExpression("'dd/MM/yyyy HH:mm:ss'")
                             }, nullable: true, new List<bool>(), type: typeof(string), typeMapping: new StringTypeMapping("", DbType.String));
                 });

            modelBuilder.HasDbFunction(typeof(CustomQuery).GetMethod(nameof(CustomQuery.ToDateString))).HasTranslation(
                e =>
                {
                    return new SqlFunctionExpression(functionName: "format", arguments: new[]{
                                 e.First(),
                                 new SqlFragmentExpression("'dd/MM/yyyy'")
                            }, nullable: true, new List<bool>(), type: typeof(string), typeMapping: new StringTypeMapping("", DbType.String));
                });
            //2.Sau đó trong linq sử dụng để filter theo serverside
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.Password).HasMaxLength(500);

                entity.Property(e => e.Search).HasMaxLength(255);

                entity.Property(e => e.Username).HasMaxLength(255);

                entity.HasOne(d => d.AccountStatus)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.AccountStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Account__Account__4E88ABD4");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Account__RoleId__59063A47");
            });

            modelBuilder.Entity<AccountBuy>(entity =>
            {
                entity.ToTable("AccountBuy");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.Password).HasMaxLength(500);

                entity.Property(e => e.Search).HasMaxLength(255);

                entity.Property(e => e.Username).HasMaxLength(255);
            });

            modelBuilder.Entity<AccountStatus>(entity =>
            {
                entity.ToTable("AccountStatus");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.ToTable("ActivityLog");

                entity.Property(e => e.Browser).HasMaxLength(255);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Device).HasMaxLength(255);

                entity.Property(e => e.EntityCode).HasMaxLength(255);

                entity.Property(e => e.Info).HasMaxLength(255);

                entity.Property(e => e.IpAddress).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Os)
                    .HasMaxLength(255)
                    .HasColumnName("OS");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.ActivityLogs)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ActivityL__Accou__4F7CD00D");
            });

            modelBuilder.Entity<AuctionProduct>(entity =>
            {
                entity.ToTable("AuctionProduct");

                entity.Property(e => e.ClosedTime).HasColumnType("datetime");

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.OpenTime).HasColumnType("datetime");

                entity.Property(e => e.RegisterClosedTime).HasColumnType("datetime");

                entity.Property(e => e.RegisterOpenTime).HasColumnType("datetime");

                entity.Property(e => e.Search).HasMaxLength(255);

                entity.HasOne(d => d.AuctionProductStatus)
                    .WithMany(p => p.AuctionProducts)
                    .HasForeignKey(d => d.AuctionProductStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuctionPr__Aucti__60A75C0F");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.AuctionProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuctionPr__Produ__5EBF139D");
            });

            modelBuilder.Entity<AuctionProductStatus>(entity =>
            {
                entity.ToTable("AuctionProductStatus");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__Accou__571DF1D5");

                entity.HasOne(d => d.NotificationStatus)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.NotificationStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__Notif__5812160E");
            });

            modelBuilder.Entity<NotificationStatus>(entity =>
            {
                entity.ToTable("NotificationStatus");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__AccountI__5AEE82B9");

                entity.HasOne(d => d.AuctionProduct)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AuctionProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__AuctionP__5FB337D6");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__OrderSta__534D60F1");

                entity.HasOne(d => d.OrderType)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__OrderTyp__5441852A");
            });

            modelBuilder.Entity<OrderAccount>(entity =>
            {
                entity.ToTable("OrderAccount");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Search).HasMaxLength(255);

                entity.HasOne(d => d.AccountBuy)
                    .WithMany(p => p.OrderAccounts)
                    .HasForeignKey(d => d.AccountBuyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderAcco__Accou__5BE2A6F2");

                entity.HasOne(d => d.OrderAccountStatus)
                    .WithMany(p => p.OrderAccounts)
                    .HasForeignKey(d => d.OrderAccountStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderAcco__Order__5DCAEF64");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderAccounts)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderAcco__Order__5CD6CB2B");

                entity.HasOne(d => d.OrderPaymentStatus)
                    .WithMany(p => p.OrderAccounts)
                    .HasForeignKey(d => d.OrderPaymentStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderAcco__Order__619B8048");
            });

            modelBuilder.Entity<OrderAccountPaymentStatus>(entity =>
            {
                entity.ToTable("OrderAccountPaymentStatus");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<OrderAccountStatus>(entity =>
            {
                entity.ToTable("OrderAccountStatus");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("OrderStatus");

                entity.Property(e => e.Color).HasMaxLength(255);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<OrderType>(entity =>
            {
                entity.ToTable("OrderType");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<OrderUpdateHistory>(entity =>
            {
                entity.ToTable("OrderUpdateHistory");

                entity.Property(e => e.Browser).HasMaxLength(255);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Device).HasMaxLength(255);

                entity.Property(e => e.IpAddress).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Os)
                    .HasMaxLength(255)
                    .HasColumnName("OS");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderUpdateHistories)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderUpda__Order__52593CB8");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.HasOne(d => d.ProductBrand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductBrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__Product__5165187F");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__Product__5070F446");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__Provinc__59FA5E80");
            });

            modelBuilder.Entity<ProductBrand>(entity =>
            {
                entity.ToTable("ProductBrand");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.ToTable("Province");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Right>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<RoleRight>(entity =>
            {
                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.Rights)
                    .WithMany(p => p.RoleRights)
                    .HasForeignKey(d => d.RightsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RoleRight__Right__5535A963");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleRights)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RoleRight__RoleI__5629CD9C");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Setting");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
