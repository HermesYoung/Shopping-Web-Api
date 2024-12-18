namespace Repositories.Repositories.ProductRepository.Models;

public class CategorizedProduct
{
    public Guid CategoryId { get; }
    public ProductDetail ProductDetail { get; }

    public CategorizedProduct(Guid categoryId, ProductDetail productDetail)
    {
        CategoryId = categoryId;
        ProductDetail = productDetail;
    }
}