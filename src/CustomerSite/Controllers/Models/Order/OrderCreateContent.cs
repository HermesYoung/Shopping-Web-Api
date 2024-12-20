namespace CustomerSite.Controllers.Models.Order;

public class OrderCreateContent
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Cart Cart { get; set; }
}