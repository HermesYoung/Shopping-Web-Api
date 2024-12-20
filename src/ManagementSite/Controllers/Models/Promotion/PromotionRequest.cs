using Repositories.Repositories.PromotionRepository.Models;
using Repositories.Repositories.PromotionRepository.Models.PromotionProviders;

namespace ManagementSite.Controllers.Models.Promotion;

public class PromotionRequest
{
    public required string Title { get; set; }
    public SpecialOfferProvider? SpecialOffer { get; set; }
    public DiscountProvider? Discount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? DisplayContent { get; set; }
}