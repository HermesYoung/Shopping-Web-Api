namespace DatabaseContext.Entities;

public partial class ProductCategory
{
    public Guid ProductId { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
