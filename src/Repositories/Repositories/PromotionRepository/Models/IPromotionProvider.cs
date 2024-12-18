namespace Repositories.Repositories.PromotionRepository.Models;

public interface IPromotionProvider
{
    public PromotionType PromotionType { get; }
    public IEnumerable<Guid> TargetProducts { get; }
    public ShoppingCart ApplyDiscount(ShoppingCart shoppingCart);
    public int GetPrice(Guid targetProductId, double originalPrice);
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

    public int GetPrice(Guid targetProductId, double originalPrice)
    {
        return (int)(ProductSpecialOfferPrices.TryGetValue(targetProductId, out var productPrice)
            ? Math.Floor(productPrice)
            : originalPrice);
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

    public int GetPrice(Guid targetProductId, double originalPrice)
    {
        if (ProductDiscountPercecnt.TryGetValue(targetProductId, out var value))
        {
            return (int)Math.Floor(value * originalPrice);
        }

        return (int)originalPrice;
    }
}