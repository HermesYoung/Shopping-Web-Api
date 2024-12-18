using System.Text.Json;
using DatabaseContext.Context;
using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstracts;
using Repositories.Common;
using Repositories.Repositories.PromotionRepository.Models;

namespace Repositories.Repositories.PromotionRepository;

public class PromotionRepository : IPromotionRepository
{
    private ShoppingWebDbContext _shoppingWebDbContext;

    public PromotionRepository(ShoppingWebDbContext shoppingWebDbContext)
    {
        _shoppingWebDbContext = shoppingWebDbContext;
    }

    public async Task<Result> CreatePromotion(PromotionContent content)
    {
        _shoppingWebDbContext.Promotions.Add(new Promotion()
        {
            Id = Guid.NewGuid(),
            Title = content.Title,
            StartDate = content.StartDate,
            EndDate = content.EndDate,
            ContentJson = JsonSerializer.Serialize(content.Content),
            DisplayContent = content.DisplayContent
        });

        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdatePromotion(Guid promotionId, PromotionContent content)
    {
        var promotion = await _shoppingWebDbContext.Promotions.FindAsync(promotionId);
        if (promotion == null)
        {
            return Result.Failure(Error.Create("Promotion not found",
                new ErrorMessage(ErrorCode.PromotionNotFound, promotionId)));
        }

        promotion.Title = content.Title;
        promotion.StartDate = content.StartDate;
        promotion.EndDate = content.EndDate;
        promotion.ContentJson = JsonSerializer.Serialize(content);
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
}