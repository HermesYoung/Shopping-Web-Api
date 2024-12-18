using DatabaseContext.Entities;
using Repositories.Common;
using Repositories.Repositories.ProductRepository.Models;

namespace Repositories.Abstracts;

public interface IProductRepository
{
    Task<Result> AddProductsAsync(IEnumerable<CategorizedProduct> categorizedProducts);
    Task<Result> DeleteProductAsync(Guid productId);
    Task<Result> UpdateProductAsync(Guid productId, ProductUpdateDetail productUpdateDetail);
    IQueryable<Product> GetProducts();
    Task<Result> ModifyProductCategoryAsync(Guid productId, IEnumerable<Guid> categoriesIds);
}