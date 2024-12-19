using Repositories.Common;
using Repositories.Repositories.OrderRepository.Models;

namespace Repositories.Abstracts;

public interface IOrderRepository
{
    Task<Result> CreateOrderAsync(OrderContent orderContent);
    Task<Result> DeleteOrderAsync(Guid orderId);
    Task<IEnumerable<OrderDetails>> GetOrderByCustomerInfoAsync(CustomerInfo customerInfo);
    Task<IEnumerable<OrderDetails>> GetOrdersAsync(OrderQuery orderQuery);
    Task<Result> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    Task<Result<OrderDetails>> GetOrderByIdAsync(Guid orderId);
}