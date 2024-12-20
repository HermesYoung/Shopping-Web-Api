using CustomerSite.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.Abstracts;
using Repositories.Common;
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
                    ? JsonSerializer.Deserialize<IEnumerable<IPromotionProvider>>(x.Promotions.First().ContentJson)!
                        .Select(provider => provider.GetPrice(x.Id, x.Price)).Min()
                    : x.Price,
            }));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var result = await _productRepository.GetProductDetailByIdAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error!.ErrorMessage.ErrorCode == ErrorCode.ProductNotFound)
                {
                    return NotFound(id);
                }

                return BadRequest(result.Error);
            }

            return Ok(result.Value!);
        }
    }
}