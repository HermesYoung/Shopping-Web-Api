using DatabaseContext.Context;
using DatabaseContext.Entities;
using Repositories.Abstracts;
using Repositories.Common;
using Repositories.Repositories.ProductRepository.Models;

namespace Repositories.Repositories.ProductRepository;

internal class ProductRepository : IProductRepository
{
    private readonly ShoppingWebDbContext _shoppingWebDbContext;

    public ProductRepository(ShoppingWebDbContext shoppingWebDbContext)
    {
        _shoppingWebDbContext = shoppingWebDbContext;
    }

    public async Task<Result> AddProducts(IEnumerable<CategorizedProduct> categorizedProducts)
    {
        var productList = new List<Product>();

        foreach (var categorizedProduct in categorizedProducts)
        {
            var category = await _shoppingWebDbContext.Categories.FindAsync(categorizedProduct.CategoryId);
            if (category == null)
            {
                return Result.Failure();
            }

            productList.Add(new Product()
            {
                Name = categorizedProduct.ProductDetail.Name,
                Price = categorizedProduct.ProductDetail.Price,
                Description = categorizedProduct.ProductDetail.Description,
                IsSoldOut =  categorizedProduct.ProductDetail.IsSoldOut,
                IsDisabled = categorizedProduct.ProductDetail.IsDisabled,
                Id = Guid.NewGuid()
            });
        }

        _shoppingWebDbContext.Products.AddRange(productList);
        await _shoppingWebDbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteProduct(Guid productId)
    {
        _shoppingWebDbContext.Products.Where(x => x.Id == productId).ToList()
            .ForEach(x => x.Categories.Clear());

        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateProduct(Guid productId, ProductUpdateDetail productUpdateDetail)
    {
        var product = _shoppingWebDbContext.Products.FirstOrDefault(x => x.Id == productId);
        if (product == null)
        {
            return Result.Failure();
        }
        product.Name = productUpdateDetail.ProductDetail.Name;
        product.Price = productUpdateDetail.ProductDetail.Price;
        product.Description = productUpdateDetail.ProductDetail.Description;
        product.IsSoldOut = productUpdateDetail.ProductDetail.IsSoldOut;
        product.IsDisabled = productUpdateDetail.ProductDetail.IsDisabled;
        
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }
}