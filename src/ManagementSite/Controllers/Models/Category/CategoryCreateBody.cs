namespace ManagementSite.Controllers.Models.Category;

public class CategoryCreateBody
{
    public required int CategoryId { get; set; }
    public int? ParentId { get; set; }
    public required string Name { get; set; }
}