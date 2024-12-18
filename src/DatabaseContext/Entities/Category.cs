namespace DatabaseContext.Entities;

public partial class Category
{
    public string Name { get; set; } = null!;

    public Guid Id { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
