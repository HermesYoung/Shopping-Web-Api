namespace Repositories.Repositories.ProductRepository.Models;

public class ProductDetail
{
    public ProductDetail(string name, string description, int price, bool isSoldOut, bool isDisabled)
    {
        Name = name;
        Description = description;
        Price = price;
        IsSoldOut = isSoldOut;
        IsDisabled = isDisabled;
    }

    public string Name { get; }
    public string? Description { get; }
    public int Price { get; }
    public bool IsSoldOut { get; }
    public bool IsDisabled { get; }
}