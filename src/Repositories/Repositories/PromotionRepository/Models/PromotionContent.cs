namespace Repositories.Repositories.PromotionRepository.Models;

public class PromotionContent
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    public required string Title { get; set; }

    public string? DisplayContent { get; set; }

    public required IEnumerable<IPromotionProvider> Content { get; set; }
}