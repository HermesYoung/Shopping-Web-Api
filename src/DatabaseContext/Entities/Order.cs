namespace DatabaseContext.Entities;

public partial class Order
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string Email { get; set; } = null!;

    public int Status { get; set; }

    public string SerialNumber { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Address { get; set; } = null!;
}
