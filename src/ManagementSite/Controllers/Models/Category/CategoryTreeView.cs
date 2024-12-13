namespace ManagementSite.Controllers.Models.Category;

public class CategoryView
{
    public int Id { get; }
    public string Name { get; }

    public CategoryView(int id, string name)
    {
        Id = id;
        Name = name;
    }
}