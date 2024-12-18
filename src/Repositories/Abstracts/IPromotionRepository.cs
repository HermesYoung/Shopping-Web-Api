using Repositories.Common;
using Repositories.Repositories.PromotionRepository.Models;

namespace Repositories.Abstracts;

public interface IPromotionRepository
{
    Task<Result> CreatePromotion(PromotionContent content);
    Task<Result> UpdatePromotion(Guid promotionId, PromotionContent content);
    Task<IEnumerable<PromotionCalender>> GetPromotionCalenderAsync(DateTime startDate, DateTime endDate);
}