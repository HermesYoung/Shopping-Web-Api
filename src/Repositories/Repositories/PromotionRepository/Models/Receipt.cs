﻿namespace Repositories.Repositories.PromotionRepository.Models;

public class Receipt
{
    public static Receipt FromProducts(IEnumerable<ShoppingCart.ProductInCart> products,
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
        
        return new Receipt(productInCarts,
            total);
    }

    public IEnumerable<ShoppingCart.ProductInCart> Items { get; set; }

    public double Total { get; set; }

    private Receipt(IEnumerable<ShoppingCart.ProductInCart> items, double total)
    {
        Items = items;
        Total = total;
    }
}