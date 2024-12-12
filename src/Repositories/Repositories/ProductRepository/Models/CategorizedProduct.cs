namespace Repositories.Repositories.ProductRepository.Models;

public class CategorizedProduct
{
    public int CategoryId { get; }
    public ProductDetail ProductDetail { get; }

    public CategorizedProduct(int categoryId, ProductDetail productDetail)
    {
        CategoryId = categoryId;
        ProductDetail = productDetail;
    }
}