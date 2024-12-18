namespace Repositories.Repositories.PromotionRepository.Models;

public interface IPromotionProvider
{
    public PromotionType PromotionType { get; }
    public ShoppingCart ApplyDiscount(ShoppingCart shoppingCart);
}

public class SpecialOfferProvider : IPromotionProvider
{
    public PromotionType PromotionType => PromotionType.SpecialOffer;
    public Dictionary<Guid, double> ProductSpecialOfferPrices { get; }

    public SpecialOfferProvider(Dictionary<Guid, double> productSpecialOfferPrices)
    {
        ProductSpecialOfferPrices = productSpecialOfferPrices;
    }
    
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
    public Dictionary<Guid, double> ProductDiscountPercecnt { get; }
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