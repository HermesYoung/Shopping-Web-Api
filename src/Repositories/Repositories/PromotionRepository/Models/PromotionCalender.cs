namespace Repositories.Repositories.PromotionRepository.Models;

public class PromotionCalender
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}