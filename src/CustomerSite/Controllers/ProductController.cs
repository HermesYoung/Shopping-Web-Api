using CustomerSite.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstracts;
using Repositories.Common;
using Repositories.Repositories.PromotionRepository.Models;
using Repositories.Repositories.PromotionRepository.Models.PromotionProviders;
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

            baseQuery = baseQuery.OrderBy(x => x.Name).Skip((int)(query.PageSize * query.PageNumber))
                .Take((int)query.PageSize);
            var products = baseQuery.ToList();
            return Ok(products.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Description = x.Description,
                DiscontPrice = x.Promotions.Any()
                    ? JsonSerializer.Deserialize<IEnumerable<PromotionProviderBase>>(x.Promotions.First().ContentJson)!
                        .Select(provider => provider.GetPrice(x.Id, x.Price)).Min()
                    : x.Price,
            }));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var result = await _productRepository.GetProductWithPromotions().FirstOrDefaultAsync(x => x.Id == id);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(new
            {
                Id = result.Id,
                Name = result.Name,
                Price = result.Price,
                Description = result.Description,
                DiscontPrice = result.Promotions.Any()
                    ? JsonSerializer.Deserialize<IEnumerable<PromotionProviderBase>>(result.Promotions.First().ContentJson)!
                        .Select(provider => provider.GetPrice(result.Id, result.Price)).Min()
                    : result.Price
            });
        }
    }
}