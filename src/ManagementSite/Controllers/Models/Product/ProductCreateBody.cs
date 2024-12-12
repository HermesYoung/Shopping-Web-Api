namespace ManagementSite.Controllers.Models.Product;

public class ProductCreateBody
{
    public required IEnumerable<ProductContent> Content { get; set; }
}