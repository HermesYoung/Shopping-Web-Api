namespace CustomerSite.Controllers.Models;

public class Cart
{
    public IEnumerable<Item> Products { get; set; }

    public class Item
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}