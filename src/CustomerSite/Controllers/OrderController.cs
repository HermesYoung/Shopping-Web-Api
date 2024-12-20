using CustomerSite.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.Abstracts;
using Repositories.Repositories.OrderRepository.Models;
using Repositories.Repositories.PromotionRepository.Models;

namespace CustomerSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IProductRepository _productRepository;

        public OrderController(IOrderRepository orderRepository, IPromotionRepository promotionRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _promotionRepository = promotionRepository;
            _productRepository = productRepository;
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
        
        [HttpPost("Preview")]
        public async Task<IActionResult> GetShoppingCartSummaryAsync([FromBody] Cart cart)
        {
            var promotions = await _promotionRepository.GetCurrentPromotionAsync();
            var productsPriceByIds =
                await _productRepository.GetProductsPriceByIds(cart.Products.Select(x => x.Id));
            var productPricesDictionary = productsPriceByIds.ToDictionary(price => price.Id, price => price);
            var shoppingCart = new ShoppingCart(cart.Products.Select(x => new ShoppingCart.ProductInCart()
            {
                Id = x.Id,
                DiscountPrice = null,
                Name = productPricesDictionary[x.Id].Name,
                Price = productPricesDictionary[x.Id].Price,
                Quantity = x.Quantity,
            }));
            var result = shoppingCart.GetReceipt(promotions);
            return Ok(result);
        }
    }
}
