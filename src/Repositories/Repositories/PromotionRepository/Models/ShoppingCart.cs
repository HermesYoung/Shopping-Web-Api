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

    public Receipt GetReceipt(IEnumerable<PromotionContent> promotions)
    {
        return Receipt.FromShoppingCart(this, promotions);
    }
}