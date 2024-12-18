namespace Repositories.Repositories.ProductRepository.Models;

public class CategorizedProduct
{
    public Guid CategoryId { get; set; }
    public ProductDetail ProductDetail { get; set; }
}