using ManagementSite.Controllers.Models.Promotion;

namespace ManagementSite.Controllers.Models;

public class PromotionUpdateRequest : PromotionCreateRequest
{
    public Guid Id { get; set; }
}