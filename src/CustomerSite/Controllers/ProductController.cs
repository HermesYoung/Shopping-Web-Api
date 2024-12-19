using ManagementSite.Controllers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Abstracts;
using Repositories.Repositories.PromotionRepository.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CustomerSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IPromotionRepository _promotionRepository;

        public ProductController(IProductRepository productRepository, IPromotionRepository promotionRepository)
        {
            _productRepository = productRepository;
            _promotionRepository = promotionRepository;
        }

        [HttpGet]
        public IActionResult GetProducts([FromQuery] ProductQuery query)
        {
            var baseQuery = _productRepository.GetProductWithPromotions().Where(x => !x.IsDisabled);
            if (query.CategoryId.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.Categories.Any(y => y.Id == query.CategoryId.Value));
            }

            baseQuery = baseQuery.OrderBy(x => x.Name).Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize);
            var products = baseQuery.ToList();
            return Ok(products.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Description = x.Description,
                DiscontPrice = x.Promotions.Any()
                    ? JsonSerializer.Deserialize<IEnumerable<IPromotionProvider>>(x.Promotions.First().ContentJson)!
                        .Select(provider => provider.GetPrice(x.Id, x.Price)).Min()
                    : x.Price,
            }));
        }

        [HttpPost("ShoppingCartSummary")]
        public async Task<IActionResult> GetShoppingCartSummaryAsync([FromBody] ShoppingCart shoppingCart)
        {
            var promotions = await _promotionRepository.GetCurrentPromotionAsync();
            var result = shoppingCart.GetSummary(promotions);
            return Ok(result);
        }
    }
}