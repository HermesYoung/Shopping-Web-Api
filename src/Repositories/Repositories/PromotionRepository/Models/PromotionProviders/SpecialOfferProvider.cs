namespace Repositories.Repositories.PromotionRepository.Models.PromotionProviders;

public class SpecialOfferProvider : PromotionProviderBase
{
    public override IEnumerable<Guid> TargetProducts => ProductSpecialOfferPrices.Keys.ToList();
    public required Dictionary<Guid, double> ProductSpecialOfferPrices { get; set; }

    public override ShoppingCart ApplyDiscount(ShoppingCart shoppingCart)
    {
        var products = shoppingCart.Products.ToList();
        foreach (var product in products)
        {
            if (ProductSpecialOfferPrices.TryGetValue(product.Id, out var productPrice))
            {
                product.DiscountPrice = productPrice;
            }
        }

        shoppingCart.Products = products;
        return shoppingCart;
    }

    public override int GetPrice(Guid targetProductId, double originalPrice)
    {
        return (int)(ProductSpecialOfferPrices.TryGetValue(targetProductId, out var productPrice)
            ? Math.Floor(productPrice)
            : originalPrice);
    }
}