namespace ManagementSite.Controllers.Models.Product;

public class ProductBody
{
    public required IEnumerable<ProductContent> Content { get; set; }
}