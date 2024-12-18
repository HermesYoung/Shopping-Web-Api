namespace Repositories.Repositories.PromotionRepository.Models;

public class ShoppingCart
{
    public IEnumerable<ProductInCart> Products { get; set; }

    public ShoppingCart(IEnumerable<ProductInCart> products)
    {
        Products = products;
    }

    public class ProductInCart
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double? DiscountPrice { get; set; }
    }

    public Summary GetSummary(IEnumerable<PromotionContent> promotions)
    {
        return Summary.FromProducts(Products, promotions);
    }
}

public class Summary
{
    public static Summary FromProducts(IEnumerable<ShoppingCart.ProductInCart> products,
        IEnumerable<PromotionContent> promotionContents)
    {
        var productInCarts = products.ToList();
        var promotions = promotionContents.SelectMany(x => x.Content).ToList();
        var total = 0;
        if (!promotions.Any())
        {
            total = (int)productInCarts.Sum(x => x.Price * x.Quantity);
        }
        else
        {
            foreach (var product in productInCarts)
            {
                var discountPrice = promotions.Select(x => x.GetPrice(product.Id ,product.Price)).Min();
                total += discountPrice * product.Quantity;
                product.DiscountPrice = Math.Abs(discountPrice - product.Price) < 0.1? null : discountPrice;
            }
        }
        
        return new Summary(productInCarts,
            total);
    }

    public IEnumerable<ShoppingCart.ProductInCart> Items { get; set; }

    public double Total { get; set; }

    private Summary(IEnumerable<ShoppingCart.ProductInCart> items, double total)
    {
        Items = items;
        Total = total;
    }
}