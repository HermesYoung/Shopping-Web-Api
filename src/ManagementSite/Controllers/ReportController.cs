using ManagementSite.Controllers.Models.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Abstracts;

namespace ManagementSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ReportController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("Product/Sell")]
        public async Task<IActionResult> GetProductSell([FromQuery] TimeQuery query)
        {
            var result = await _productRepository.GetProductSellSummary(query.StartTime, query.EndTime);
            return Ok(result);
        }
    }
}
