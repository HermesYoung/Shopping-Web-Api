using Repositories.Common;
using Repositories.Repositories.ProductRepository.Models;

namespace Repositories.Abstracts;

public interface IProductRepository
{
    Task<Result> AddProductsAsync(IEnumerable<CategorizedProduct> categorizedProducts);
    Task<Result> DeleteProductAsync(Guid productId);
    Task<Result> UpdateProductAsync(Guid productId, ProductUpdateDetail productUpdateDetail);
}