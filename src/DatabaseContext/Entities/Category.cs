namespace DatabaseContext.Entities;

public partial class Category
{
    public int Id { get; set; }

    public int? Parent { get; set; }

    public string Name { get; set; } = null!;
}
