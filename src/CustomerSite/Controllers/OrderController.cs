using DatabaseContext.Entities;
using Microsoft.AspNetCore.Mvc;
using Repositories.Abstracts;
using Repositories.Repositories.OrderRepository.Models;

namespace CustomerSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync([FromQuery] CustomerInfo customerInfo)
        {
            var result = await _orderRepository.GetOrderByCustomerInfoAsync(customerInfo);
            
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] Guid id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (!order.IsSuccess)
            {
                return NotFound(order.Error);
            }
            
            return Ok(order.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderContent order)
        {
            var result = await _orderRepository.CreateOrderAsync(order);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }
    }
}
