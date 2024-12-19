namespace ManagementSite.Controllers.Models;

public class ProductQuery
{
    public Guid? CategoryId { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}