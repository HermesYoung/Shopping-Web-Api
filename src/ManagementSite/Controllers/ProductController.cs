using DatabaseContext.Entities;
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
        public IActionResult CreateProduct(IEnumerable<CategorizedProduct> categorizedProducts)
        {
            _productRepository.AddProductsAsync(categorizedProducts);
            return Ok();
        }
    }
}
