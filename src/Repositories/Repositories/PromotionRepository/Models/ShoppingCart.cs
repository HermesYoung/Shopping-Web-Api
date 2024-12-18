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
    }

    public Summary GetSummary()
    {
        return Summary.FromProducts(Products);
    }
}

public class Summary
{
    public static Summary FromProducts(IEnumerable<ShoppingCart.ProductInCart> products)
    {
        var productInCarts = products.ToList();
        return new Summary(productInCarts, productInCarts.Sum(p => p.Price * p.Quantity));
    }

    public IEnumerable<ShoppingCart.ProductInCart> Items { get; set; }

    public double Total { get; set; }

    private Summary(IEnumerable<ShoppingCart.ProductInCart> items, double total)
    {
        Items = items;
        Total = total;
    }
}