namespace DatabaseContext.Entities;

public partial class OrderContent
{
    public string OrderSn { get; set; } = null!;

    public Guid Product { get; set; }

    public int Price { get; set; }

    public int Promotion { get; set; }

    public virtual Order OrderSnNavigation { get; set; } = null!;

    public virtual Product ProductNavigation { get; set; } = null!;

    public virtual Promotion PromotionNavigation { get; set; } = null!;
}
