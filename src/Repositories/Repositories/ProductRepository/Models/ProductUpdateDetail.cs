namespace Repositories.Repositories.ProductRepository.Models;

public class ProductUpdateDetail
{
    public Guid ProductId { get; }
    public ProductDetail ProductDetail { get; }

    public ProductUpdateDetail(Guid productId, ProductDetail productDetail)
    {
        ProductId = productId;
        ProductDetail = productDetail;
    }
}