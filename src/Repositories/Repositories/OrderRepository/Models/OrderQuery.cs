namespace Repositories.Repositories.OrderRepository.Models;

public class OrderQuery
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? Status { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}