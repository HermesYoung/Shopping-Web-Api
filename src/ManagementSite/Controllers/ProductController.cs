using ManagementSite.Controllers.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstracts;
using Repositories.Repositories.ProductRepository.Models;

namespace ManagementSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductCreateBody body)
        {
            var categorizedProducts = body.Content
                .SelectMany(productContent => productContent.ProductDetails.Select(productDetail =>
                    new CategorizedProduct(productContent.CategoryId, productDetail))).ToList();
            var result = await _productRepository.AddProductsAsync(categorizedProducts);
            if (result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQuery query)
        {
            var products = _productRepository.GetProducts();
            if (query.CategoryId.HasValue)
            {
                products = products.Where(x => x.Categories.Any(category => category.Id == query.CategoryId));
            }
            
            products = products.OrderBy(x => x.Id).Skip(query.PageSize * query.PageNumber).Take(query.PageSize);

            var productList = await products.ToListAsync();
            return Ok(productList.Select(product => new
            {
                product.Id,
                product.Name,
                product.Price,
                product.Description,
                Category = product.Categories.FirstOrDefault(),
                product.IsDisabled,
                product.IsSoldOut
            }));
        }
    }

    public class ProductQuery
    {
        public Guid? CategoryId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}