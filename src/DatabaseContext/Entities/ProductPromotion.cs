namespace DatabaseContext.Entities;

public partial class ProductPromotion
{
    public Guid ProductId { get; set; }

    public int PromotionId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Promotion Promotion { get; set; } = null!;
}
