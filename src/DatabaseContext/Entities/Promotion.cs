namespace DatabaseContext.Entities;

public partial class Promotion
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string ContentJson { get; set; } = null!;

    public int Title { get; set; }

    public string? DisplayContent { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
