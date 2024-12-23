﻿using DatabaseContext.Context;
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

    public async Task<Result> UpdateProductAsync(ProductUpdateDetail productUpdateDetail)
    {
        var product = _shoppingWebDbContext.Products.FirstOrDefault(x => x.Id == productUpdateDetail.ProductId);
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

    public async Task<Result> ModifyProductCategoryAsync(Guid productId, IEnumerable<Guid> categoriesIds)
    {
        var categories = await _shoppingWebDbContext.Categories.Where(x => categoriesIds.Contains(x.Id)).ToListAsync();
        var product = _shoppingWebDbContext.Products.FirstOrDefault(x => x.Id == productId);
        if (product == null)
        {
            return Result.Failure(Error.Create("Product not found",
                new ErrorMessage(ErrorCode.ProductNotFound, default)));
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

    public IQueryable<Product> GetProductWithPromotions()
    {
        return _shoppingWebDbContext.Products.Include(x => x.Categories).Include(x => x.Promotions).AsQueryable();
    }

    public async Task<IEnumerable<ProductPrice>> GetProductsPriceByIds(IEnumerable<Guid> productIds)
    {
        var products = await _shoppingWebDbContext.Products.AsQueryable().Where(x => productIds.Contains(x.Id))
            .ToListAsync();
        return products.Select(x => new ProductPrice()
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price,
        });
    }

    public async Task<Result<ProductDetail>> GetProductDetailByIdAsync(Guid productId)
    {
        var product = await _shoppingWebDbContext.Products.Include(x => x.Categories).AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == productId);
        if (product == null)
        {
            return Result<ProductDetail>.Failure(Error.Create("Product not found",
                new ErrorMessage(ErrorCode.ProductNotFound)));
        }

        return Result<ProductDetail>.Success(new ProductDetail()
        {
            Description = product.Description,
            Name = product.Name,
            Price = product.Price,
            IsSoldOut = product.IsSoldOut,
            IsDisabled = product.IsDisabled,
        });
    }

    public async Task<SellingSummary> GetProductSellSummary(DateTime startDate, DateTime endDate)
    {
        var sellList = await _shoppingWebDbContext.ProductSells.Include(x => x.Product).AsQueryable()
            .Where(x => x.Date >= startDate && x.Date <= endDate).ToListAsync();

        var sellingSummary = sellList.GroupBy(x => x.ProductId).Select(sells => new ProductSellSummary
        {
            Id = sells.Key,
            Name = sells.First().Product.Name,
            Quantity = sells.Sum(x => x.Quantity),
            Total = sells.Sum(x => x.TotalPrice)
        });

        return new SellingSummary
        {
            From = startDate,
            To = endDate,
            ProductSellsSummary = sellingSummary
        };
    }
}

public class SellingSummary
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public IEnumerable<ProductSellSummary> ProductSellsSummary { get; set; }
}

public class ProductSellSummary
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
}