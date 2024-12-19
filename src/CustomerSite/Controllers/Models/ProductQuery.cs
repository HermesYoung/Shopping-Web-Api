namespace CustomerSite.Controllers.Models;

public class ProductQuery
{
    public Guid? CategoryId { get; set; }
    public uint PageSize { get; set; }
    public uint PageNumber { get; set; }
}