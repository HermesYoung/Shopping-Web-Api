using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext.Context;

public partial class ShoppingWebDbContext : DbContext
{
    public ShoppingWebDbContext(DbContextOptions<ShoppingWebDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductSell> ProductSells { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pk");

            entity.ToTable("category", "shopping_web");

            entity.HasIndex(e => e.Name, "category_name_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_pk");

            entity.ToTable("order", "shopping_web");

            entity.HasIndex(e => e.CreateDate, "order_create_date_index");

            entity.HasIndex(e => new { e.Name, e.Email, e.Phone }, "order_name_email_phone_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.ContentJson)
                .IsUnicode(false)
                .HasColumnName("content_json");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
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
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pk");

            entity.ToTable("product", "shopping_web");

            entity.HasIndex(e => e.IsDisabled, "product_is_disabled_index");

            entity.HasIndex(e => e.IsSoldOut, "product_is_visible_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IsDisabled).HasColumnName("is_disabled");
            entity.Property(e => e.IsSoldOut).HasColumnName("is_sold_out");
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
                        j.IndexerProperty<Guid>("CategoryId").HasColumnName("category_id");
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
                        j.IndexerProperty<Guid>("PromotionId").HasColumnName("promotion_id");
                    });
        });

        modelBuilder.Entity<ProductSell>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_sell_pk");

            entity.ToTable("product_sell", "shopping_web");

            entity.HasIndex(e => e.Date, "product_sell_date_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("total_price");

            entity.HasOne(d => d.Order).WithMany(p => p.ProductSells)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_sell_order_id_fk");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSells)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_sell_product_id_fk");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("promotion_pk");

            entity.ToTable("promotion", "shopping_web");

            entity.HasIndex(e => e.EndDate, "promotion_end_date_index");

            entity.HasIndex(e => e.StartDate, "promotion_start_date_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
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
            entity.Property(e => e.Title)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
