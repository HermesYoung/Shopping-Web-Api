using DatabaseContext.Entities;
using ManagementSite.Controllers.Models.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
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
    }
}