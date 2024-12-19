using Repositories.Repositories.PromotionRepository.Models;

namespace Repositories.Repositories.OrderRepository.Models;

public class OrderContent
{
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public Receipt Receipt { get; set; }
}

public class OrderDetails : OrderContent
{
    public required Guid Id { get; set; }
    public required OrderStatus OrderStatus { get; set; }
}