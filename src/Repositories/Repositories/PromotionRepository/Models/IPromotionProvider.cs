namespace Repositories.Repositories.PromotionRepository.Models;

public interface IPromotionProvider
{
    public PromotionType PromotionType { get; }
    public IEnumerable<Guid> TargetProducts { get; }
    public ShoppingCart ApplyDiscount(ShoppingCart shoppingCart);
}

public class SpecialOfferProvider : IPromotionProvider
{
    public PromotionType PromotionType => PromotionType.SpecialOffer;
    public IEnumerable<Guid> TargetProducts => ProductSpecialOfferPrices.Keys.ToList();
    public required Dictionary<Guid, double> ProductSpecialOfferPrices { get; set; }

    public ShoppingCart ApplyDiscount(ShoppingCart shoppingCart)
    {
        var products = shoppingCart.Products.ToList();
        foreach (var product in products)
        {
            if (ProductSpecialOfferPrices.TryGetValue(product.Id, out var productPrice))
            {
                product.Price = productPrice;
            }
        }

        shoppingCart.Products = products;
        return shoppingCart;
    }
}

public class DiscountProvider : IPromotionProvider
{
    public PromotionType PromotionType => PromotionType.Discount;
    public IEnumerable<Guid> TargetProducts => ProductDiscountPercecnt.Keys.ToList();
    public required Dictionary<Guid, double> ProductDiscountPercecnt { get; set; }

    public ShoppingCart ApplyDiscount(ShoppingCart shoppingCart)
    {
        var products = shoppingCart.Products.ToList();
        foreach (var product in products)
        {
            if (ProductDiscountPercecnt.TryGetValue(product.Id, out var value))
            {
                product.Price = value * product.Price;
            }
        }

        shoppingCart.Products = products;
        return shoppingCart;
    }
}