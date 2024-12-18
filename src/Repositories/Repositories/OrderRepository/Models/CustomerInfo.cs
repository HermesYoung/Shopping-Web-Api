namespace Repositories.Repositories.OrderRepository.Models;

public class CustomerInfo
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public int PageSize { get; set; }
    public int Page { get; set; }
}