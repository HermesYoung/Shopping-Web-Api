using System.Text.Json;
using DatabaseContext.Context;
using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstracts;
using Repositories.Common;
using Repositories.Repositories.PromotionRepository.Models;
using Repositories.Repositories.PromotionRepository.Models.PromotionProviders;

namespace Repositories.Repositories.PromotionRepository;

public class PromotionRepository : IPromotionRepository
{
    private ShoppingWebDbContext _shoppingWebDbContext;

    public PromotionRepository(ShoppingWebDbContext shoppingWebDbContext)
    {
        _shoppingWebDbContext = shoppingWebDbContext;
    }

    public async Task<Result> CreatePromotionAsync(PromotionContent content)
    {
        var result = CheckProductExistsAsync(content);
        if (!result.IsSuccess)
        {
            return Result.Failure(result.Error!);
        }

        var contentJson = JsonSerializer.Serialize<IEnumerable<PromotionProviderBase>>(content.Content);
        _shoppingWebDbContext.Promotions.Add(new Promotion()
        {
            Id = Guid.NewGuid(),
            Title = content.Title,
            StartDate = content.StartDate,
            EndDate = content.EndDate,
            ContentJson = contentJson,
            DisplayContent = content.DisplayContent,
            Products = result.Value!
        });

        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    private Result<List<Product>> CheckProductExistsAsync(PromotionContent content)
    {
        var productIds = content.Content.SelectMany(x => x.TargetProducts).Distinct().ToList();
        var products = _shoppingWebDbContext.Products.Where(x => productIds.Contains(x.Id)).ToList();
        if (products.Count != productIds.Count)
        {
            return Result<List<Product>>.Failure(Error.Create("Some products do not exist!",
                new ErrorMessage(ErrorCode.ProductNotFound)));
        }

        return Result<List<Product>>.Success(products);
    }

    public async Task<Result> UpdatePromotionAsync(Guid promotionId, PromotionContent content)
    {
        var productCheckResult = CheckProductExistsAsync(content);
        if (!productCheckResult.IsSuccess)
        {
            return Result.Failure(productCheckResult.Error!);
        }

        var promotion = await _shoppingWebDbContext.Promotions.Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == promotionId);
        if (promotion == null)
        {
            return Result.Failure(Error.Create("Promotion not found",
                new ErrorMessage(ErrorCode.PromotionNotFound, promotionId)));
        }

        promotion.Title = content.Title;
        promotion.StartDate = content.StartDate;
        promotion.EndDate = content.EndDate;
        promotion.ContentJson = JsonSerializer.Serialize(content);
        promotion.DisplayContent = content.DisplayContent;
        promotion.Products = productCheckResult.Value!;
        
        _shoppingWebDbContext.Promotions.Update(promotion);
        await _shoppingWebDbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<IEnumerable<PromotionCalender>> GetPromotionCalenderAsync(DateTime startDate, DateTime endDate)
    {
        var promotions = await _shoppingWebDbContext.Promotions.AsQueryable().Where(promotion =>
            promotion.StartDate >= startDate && promotion.StartDate <= endDate ||
            promotion.EndDate >= startDate && promotion.EndDate <= endDate).ToListAsync();

        return promotions.Select(x => new PromotionCalender()
        {
            Id = x.Id,
            Title = x.Title,
            StartDate = x.StartDate,
            EndDate = x.EndDate
        });
    }

    public async Task<Result<PromotionDetail>> GetPromotionDetailAsync(Guid promotionId)
    {
        var promotion = await _shoppingWebDbContext.Promotions.FirstOrDefaultAsync(x => x.Id == promotionId);
        if (promotion == null)
        {
            return Result<PromotionDetail>.Failure(Error.Create("Promotion not found", new ErrorMessage(ErrorCode.PromotionNotFound)));
        }
        
        return Result<PromotionDetail>.Success(new PromotionDetail()
        {
            Id = promotion.Id,
            Title = promotion.Title,
            DisplayContent = promotion.DisplayContent,
            PromotionContent = JsonSerializer.Deserialize<IEnumerable<PromotionProviderBase>>(promotion.ContentJson)!,
            StartDate = promotion.StartDate,
            EndDate = promotion.EndDate
        });
    }

    public async Task<Result> DeletePromotionAsync(Guid promotionId)
    {
        var promotion = await _shoppingWebDbContext.Promotions.Include(x => x.Products).FirstOrDefaultAsync();
        if (promotion == null)
        {
            return Result.Failure(Error.Create("Promotion not found",
                new ErrorMessage(ErrorCode.PromotionNotFound, promotionId)));
        }
        
        promotion.Products.Clear();
        _shoppingWebDbContext.Promotions.Update(promotion);
        _shoppingWebDbContext.Promotions.Remove(promotion);
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<IEnumerable<PromotionContent>> GetCurrentPromotionAsync()
    {
        var promotions = await _shoppingWebDbContext.Promotions.Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).ToListAsync();
        return promotions.Select(promotion => new PromotionContent()
        {
            Title = promotion.Title,
            StartDate = promotion.StartDate,
            EndDate = promotion.EndDate,
            DisplayContent = promotion.DisplayContent,
            Content = JsonSerializer.Deserialize<List<PromotionProviderBase>>(promotion.ContentJson)!
        });
    }
}