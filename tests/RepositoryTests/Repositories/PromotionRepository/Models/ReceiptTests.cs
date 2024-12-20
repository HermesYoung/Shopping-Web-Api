using DatabaseContext.Entities;
using Repositories.Repositories.PromotionRepository.Models;
using Repositories.Repositories.PromotionRepository.Models.PromotionProviders;

namespace RepositoryTests.Repositories.PromotionRepository.Models;

public class ReceiptTests
{
    [Test]
    public void FromShoppingCart_CreateFromShoppingCartWithNoPromotion_ReturnCorrectReceipt()
    {
        var products = new List<ShoppingCart.ProductInCart>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Price = 10,
                Quantity = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Price = 20,
                Quantity = 3
            }
        };
        var shoppingCart = new ShoppingCart(products);


        var result = Receipt.FromShoppingCart(shoppingCart, new List<PromotionContent>());
        
        Assert.That(result.Total, Is.EqualTo(70));
        Assert.That(result.Items, Is.EqualTo(products));
    }

    [Test]
    public void FromShoppingCart_CreateFromShoppingCartWithPromotion_ReturnCorrectReceipt()
    {
        var products = new List<ShoppingCart.ProductInCart>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Price = 10,
                Quantity = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Price = 20,
                Quantity = 3
            }
        };
        var shoppingCart = new ShoppingCart(products);

        var promotions = new List<PromotionContent>()
        {
            new PromotionContent
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                Title = "Test",
                DisplayContent = "test",
                Content = new List<PromotionProviderBase>()
                {
                    new DiscountProvider()
                    {
                        ProductDiscountPercent = new Dictionary<Guid, double>()
                        {
                            { products[0].Id, 0.8 },
                        }
                    }
                }
            },
            new PromotionContent()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                Title = "Test",
                DisplayContent = "test",
                Content = new List<PromotionProviderBase>()
                {
                    new SpecialOfferProvider
                    {
                        ProductSpecialOfferPrices = new Dictionary<Guid, double>()
                        {
                            { products[1].Id, 5 },
                        }
                    }
                }
            }
        };
        
        var result = Receipt.FromShoppingCart(shoppingCart, promotions);
        
        Assert.That(result.Total, Is.EqualTo(23));
    }
}