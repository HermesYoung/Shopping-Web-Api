using DatabaseContext.Context;
using DatabaseContext.Entities;
using Repositories.Common;

namespace Repositories.Repositories.ProductRepository;

public interface IProductRepository
{
    Task<Result> AddProduct(int categoryId, Product product);
    Task<Result> DeleteProduct(Product product);
    Task<Result> UpdateProduct(Product product);
}

internal class ProductRepository : IProductRepository
{
    private readonly ShoppingWebDbContext _shoppingWebDbContext;

    public ProductRepository(ShoppingWebDbContext shoppingWebDbContext)
    {
        _shoppingWebDbContext = shoppingWebDbContext;
    }

    public async Task<Result> AddProduct(int categoryId ,Product product)
    {
        var category = await _shoppingWebDbContext.Categories.FindAsync(categoryId);
        if (category == null)
        {
            return Result.Failure();
        }
        product.Categories.Add(category);
        _shoppingWebDbContext.Products.Add(product);
        _shoppingWebDbContext.ChangeTracker.DetectChanges();
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteProduct(Product product)
    {
        _shoppingWebDbContext.Products.Remove(product);
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateProduct(Product product)
    {
        _shoppingWebDbContext.Products.Update(product);
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }
}