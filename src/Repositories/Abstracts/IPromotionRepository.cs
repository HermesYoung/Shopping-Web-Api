using Repositories.Common;
using Repositories.Repositories.PromotionRepository.Models;

namespace Repositories.Abstracts;

public interface IPromotionRepository
{
    Task<Result> CreatePromotionAsync(PromotionContent content);
    Task<Result> UpdatePromotionAsync(Guid promotionId, PromotionContent content);
    Task<IEnumerable<PromotionCalender>> GetPromotionCalenderAsync(DateTime startDate, DateTime endDate);
    Task<Result> DeletePromotionAsync(Guid promotionId);
}