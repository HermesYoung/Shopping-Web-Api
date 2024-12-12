using Repositories.Common;
using Repositories.Repositories.ProductRepository.Models;

namespace Repositories.Abstracts;

public interface IProductRepository
{
    Task<Result> AddProducts(IEnumerable<CategorizedProduct> categorizedProducts);
    Task<Result> DeleteProduct(Guid productId);
    Task<Result> UpdateProduct(Guid productId, ProductUpdateDetail productUpdateDetail);
}