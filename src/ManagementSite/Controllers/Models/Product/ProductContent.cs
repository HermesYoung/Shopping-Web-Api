using Repositories.Repositories.ProductRepository.Models;

namespace ManagementSite.Controllers.Models.Product;

public class ProductContent
{
    public int CategoryId { get; set; }
    public required IEnumerable<ProductDetail> ProductDetails { get; set; }
}