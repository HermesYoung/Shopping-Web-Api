namespace Repositories.Repositories.PromotionRepository.Models;

public class Receipt
{
    public static Receipt FromShoppingCart(ShoppingCart shoppingCart, IEnumerable<PromotionContent> promotionContents)
    {
        var total = 0;
        var contents = promotionContents.ToList();
        if (!contents.Any())
        {
            total = (int)shoppingCart.Products.Select(x => x.Price).Sum();
        }

        var promotionProviders = contents.SelectMany(x => x.Content).ToList();

        var cart = promotionProviders.Aggregate(shoppingCart, (current, provider) => provider.ApplyDiscount(current));
        total = (int)cart.Products.Select(x => x.DiscountPrice ?? x.Price).Sum();

        return new Receipt(cart.Products, total);
    }

    public IEnumerable<ShoppingCart.ProductInCart> Items { get; set; }

    public double Total { get; set; }

    private Receipt(IEnumerable<ShoppingCart.ProductInCart> items, double total)
    {
        Items = items;
        Total = total;
    }
}