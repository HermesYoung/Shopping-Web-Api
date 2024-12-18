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
        
        _shoppingWebDbContext.Products.Update(product);
        
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> ModifyProductCategoryAsync(Guid productId ,IEnumerable<Guid> categoriesIds)
    {
        var categories = await _shoppingWebDbContext.Categories.Where(x => categoriesIds.Contains(x.Id)).ToListAsync();
        var product = _shoppingWebDbContext.Products.FirstOrDefault(x => x.Id == productId);
        if (product == null)
        {
            return Result.Failure(Error.Create("Product not found", new ErrorMessage(ErrorCode.ProductNotFound, default)));
        }
        
        product.Categories = categories;
        _shoppingWebDbContext.Products.Update(product);
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }
    
    public IQueryable<Product> GetProducts()
    {
       return _shoppingWebDbContext.Products.Include(x => x.Categories).AsQueryable();
    }
}