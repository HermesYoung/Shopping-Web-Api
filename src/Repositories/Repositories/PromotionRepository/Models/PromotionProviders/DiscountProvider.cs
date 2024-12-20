namespace Repositories.Repositories.PromotionRepository.Models.PromotionProviders;

public class DiscountProvider : PromotionProviderBase
{
    public override IEnumerable<Guid> TargetProducts => ProductDiscountPercent.Keys.ToList();
    public required Dictionary<Guid, double> ProductDiscountPercent { get; set; }

    public override ShoppingCart ApplyDiscount(ShoppingCart shoppingCart)
    {
        var products = shoppingCart.Products.ToList();
        foreach (var product in products)
        {
            if (ProductDiscountPercent.TryGetValue(product.Id, out var value))
            {
                var productDiscountPrice = value * product.Price;
                product.DiscountPrice = product.DiscountPrice.HasValue
                    ? Math.Min(productDiscountPrice, product.DiscountPrice.Value)
                    : productDiscountPrice;
            }
        }

        shoppingCart.Products = products;
        return shoppingCart;
    }

    public override int GetPrice(Guid targetProductId, double originalPrice)
    {
        if (ProductDiscountPercent.TryGetValue(targetProductId, out var value))
        {
            return (int)Math.Floor(value * originalPrice);
        }

        return (int)originalPrice;
    }
}