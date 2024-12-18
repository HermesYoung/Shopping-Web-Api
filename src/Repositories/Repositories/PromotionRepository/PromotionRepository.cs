using System.Text.Json;
using DatabaseContext.Context;
using DatabaseContext.Entities;
using Repositories.Common;

namespace Repositories.Repositories.PromotionRepository;

public class PromotionRepository
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
            ContentJson = JsonSerializer.Serialize(content),
            DisplayContent = content.DisplayContent
        });
        
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }
    
}

public class PromotionContent
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    public int Title { get; set; }

    public string? DisplayContent { get; set; }
    
    public object Content { get; set; }
}