using DatabaseContext.Entities;
using ManagementSite.Controllers.Models.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Abstracts;
using Repositories.Repositories.OrderRepository.Models;

namespace ManagementSite.Controllers
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
        public async Task<IActionResult> GetOrders([FromQuery] OrderQuery query)
        {
            var orders = await _orderRepository.GetOrdersAsync(query);
            return Ok(orders);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] Guid id, OrderStatusBody orderStatus)
        {
            var status = OrderStatus.TryParse(orderStatus.Status);
            if (!status.IsSuccess)
            {
                return BadRequest(status.Error);
            }
            
            var result = await _orderRepository.UpdateOrderStatusAsync(id, status.Value!);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (!order.IsSuccess)
            {
                return NotFound(order.Error);
            }
            
            return Ok(order.Value);
        }
    }
}
