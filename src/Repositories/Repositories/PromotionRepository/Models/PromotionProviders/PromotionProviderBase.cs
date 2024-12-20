using System.Text.Json.Serialization;

namespace Repositories.Repositories.PromotionRepository.Models.PromotionProviders;

[JsonDerivedType(typeof(SpecialOfferProvider), 0)]
[JsonDerivedType(typeof(DiscountProvider), 1)]
public abstract class PromotionProviderBase
{
    public abstract IEnumerable<Guid> TargetProducts { get; }
    public abstract ShoppingCart ApplyDiscount(ShoppingCart shoppingCart);
    public abstract int GetPrice(Guid targetProductId, double originalPrice);
}