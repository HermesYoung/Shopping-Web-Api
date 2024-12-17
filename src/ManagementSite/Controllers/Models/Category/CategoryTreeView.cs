namespace ManagementSite.Controllers.Models.Category;

public class CategoryResponse(int id, string name)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
}