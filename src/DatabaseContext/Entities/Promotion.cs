namespace DatabaseContext.Entities;

public partial class Promotion
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string ContentJson { get; set; } = null!;

    public string? DisplayContent { get; set; }

    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
}
