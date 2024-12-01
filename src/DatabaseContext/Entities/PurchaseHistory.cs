namespace DatabaseContext.Entities;

public partial class PurchaseHistory
{
    public int ProductId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public int OrderId { get; set; }
}
