namespace DatabaseContext.Entities;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Price { get; set; }

    public bool IsSoldOut { get; set; }

    public bool IsDisabled { get; set; }

    public virtual ICollection<ProductSell> ProductSells { get; set; } = new List<ProductSell>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
