using DatabaseContext.Entities;
using Repositories.Common;
using Repositories.Repositories.ProductRepository;
using Repositories.Repositories.ProductRepository.Models;

namespace Repositories.Abstracts;

public interface IProductRepository
{
    Task<Result> AddProductsAsync(IEnumerable<CategorizedProduct> categorizedProducts);
    Task<Result> DeleteProductAsync(Guid productId);
    Task<Result> UpdateProductAsync(ProductUpdateDetail productUpdateDetail);
    IQueryable<Product> GetProducts();
    Task<Result> ModifyProductCategoryAsync(Guid productId, IEnumerable<Guid> categoriesIds);
    IQueryable<Product> GetProductWithPromotions();
    Task<IEnumerable<ProductPrice>> GetProductsPriceByIds(IEnumerable<Guid> productIds);
    Task<Result<ProductDetail>> GetProductDetailByIdAsync(Guid productId);
    Task<SellingSummary> GetProductSellSummary(DateTime startDate, DateTime endDate);
}