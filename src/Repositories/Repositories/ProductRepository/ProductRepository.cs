using DatabaseContext.Context;
using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;
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

    public async Task<Result> AddProductsAsync(IEnumerable<CategorizedProduct> categorizedProducts)
    {
        var productList = new List<Product>();

        foreach (var categorizedProduct in categorizedProducts)
        {
            var category = await _shoppingWebDbContext.Categories.FindAsync(categorizedProduct.CategoryId);
            if (category == null)
            {
                return Result.Failure();
            }

            var product = new Product()
            {
                Name = categorizedProduct.ProductDetail.Name,
                Price = categorizedProduct.ProductDetail.Price,
                Description = categorizedProduct.ProductDetail.Description,
                IsSoldOut = categorizedProduct.ProductDetail.IsSoldOut,
                IsDisabled = categorizedProduct.ProductDetail.IsDisabled,
                Id = Guid.NewGuid(),
            };
            product.Categories.Add(category);

            productList.Add(product);
        }

        _shoppingWebDbContext.Products.AddRange(productList);
        await _shoppingWebDbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteProductAsync(Guid productId)
    {
        var product = await _shoppingWebDbContext.Products.Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == productId);
        if (product == null)
        {
            return Result.Failure();
        }

        product.Categories.Clear();
        _shoppingWebDbContext.Products.Update(product);

        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateProductAsync(Guid productId, ProductUpdateDetail productUpdateDetail)
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

    public async Task<IEnumerable<Product>> GetProductsAsync(int page, int pageSize)
    {
        var products = await _shoppingWebDbContext.Products.Include(x => x.Categories).Where(x => x.Categories.Any())
            .OrderBy(x => x.Categories.First().Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return products;
    }

    public async Task<Result<Product>> GetProductByIdAsync(Guid productId)
    {
        var product = await _shoppingWebDbContext.Products.Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == productId);
        if (product == null)
        {
            return Result<Product>.Failure(Error.Create("Product not found",
                new ErrorMessage(ErrorCode.ProductNotFound, default)));
        }

        return Result<Product>.Success(product);
    }
}