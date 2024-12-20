using Repositories.Repositories.PromotionRepository.Models.PromotionProviders;

namespace Repositories.Repositories.PromotionRepository.Models;

public class PromotionDetail
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? DisplayContent { get; set; }
    public IEnumerable<PromotionProviderBase> PromotionContent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}