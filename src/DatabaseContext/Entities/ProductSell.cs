namespace DatabaseContext.Entities;

public partial class ProductSell
{
    public DateTime Date { get; set; }

    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Product Product { get; set; } = null!;
}
