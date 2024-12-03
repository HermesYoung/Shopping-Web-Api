using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext.Context;

public partial class ShoppingWebDbContext : DbContext
{
    public ShoppingWebDbContext()
    {
    }

    public ShoppingWebDbContext(DbContextOptions<ShoppingWebDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<OperationLog> OperationLogs { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderContent> OrderContents { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<PurchaseHistory> PurchaseHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pk");

            entity.ToTable("category", "shopping_web");

            entity.HasIndex(e => e.Name, "category_name_index");

            entity.HasIndex(e => e.Parent, "category_parent_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Parent).HasColumnName("parent");
        });

        modelBuilder.Entity<OperationLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("operation_log_pk");

            entity.ToTable("operation_log", "shopping_web");

            entity.HasIndex(e => e.Date, "operation_log_date_index");

            entity.HasIndex(e => e.Operation, "operation_log_operation_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Operation)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("operation");
            entity.Property(e => e.StatusCode).HasColumnName("status_code");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.OperationLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("operation_log_user_id_fk");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_pk");

            entity.ToTable("order", "shopping_web");

            entity.HasIndex(e => e.SerialNumber, "order_serial_number_uindex").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("serial_number");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<OrderContent>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("order_content", "shopping_web");

            entity.Property(e => e.OrderSn)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("order_sn");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Product).HasColumnName("product");
            entity.Property(e => e.Promotion).HasColumnName("promotion");

            entity.HasOne(d => d.OrderSnNavigation).WithMany()
                .HasPrincipalKey(p => p.SerialNumber)
                .HasForeignKey(d => d.OrderSn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_content_order_serial_number_fk");

            entity.HasOne(d => d.ProductNavigation).WithMany()
                .HasForeignKey(d => d.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_content_product_id_fk");

            entity.HasOne(d => d.PromotionNavigation).WithMany()
                .HasForeignKey(d => d.Promotion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_content_promotion_id_fk");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pk");

            entity.ToTable("product", "shopping_web");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasMany(d => d.Categories).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("product_category_category_id_fk"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("product_category_product_id_fk"),
                    j =>
                    {
                        j.HasKey("ProductId", "CategoryId").HasName("product_category_pk");
                        j.ToTable("product_category", "shopping_web");
                        j.IndexerProperty<Guid>("ProductId").HasColumnName("product_id");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("category_id");
                    });

            entity.HasMany(d => d.Promotions).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductPromotion",
                    r => r.HasOne<Promotion>().WithMany()
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("product_promotion_promotion_id_fk"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("product_promotion_product_id_fk"),
                    j =>
                    {
                        j.HasKey("ProductId", "PromotionId").HasName("product_promotion_pk");
                        j.ToTable("product_promotion", "shopping_web");
                        j.IndexerProperty<Guid>("ProductId").HasColumnName("product_id");
                        j.IndexerProperty<int>("PromotionId").HasColumnName("promotion_id");
                    });
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("promotion_pk");

            entity.ToTable("promotion", "shopping_web");

            entity.HasIndex(e => e.EndDate, "promotion_end_date_index");

            entity.HasIndex(e => e.StartDate, "promotion_start_date_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentJson)
                .IsUnicode(false)
                .HasColumnName("content_json");
            entity.Property(e => e.DisplayContent)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("display_content");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<PurchaseHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable(" purchase_history", "shopping_web");

            entity.HasIndex(e => e.PurchaseDate, "purchase_history_purchase_date_index");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.PurchaseDate)
                .HasColumnType("datetime")
                .HasColumnName("purchase_date");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pk");

            entity.ToTable("user", "shopping_web");

            entity.HasIndex(e => e.Name, "user_pk_2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
