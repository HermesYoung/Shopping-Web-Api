namespace Repositories.Repositories.ProductRepository.Models;

public class ProductDetail
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public bool IsSoldOut { get; set; }
    public bool IsDisabled { get; set; }
}