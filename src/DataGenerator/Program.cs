using DatabaseContext.Context;
using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Extensions;
using Repositories.Repositories.ProductRepository;

namespace DataGenerator;

public class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddDbContext<ShoppingWebDbContext>(builder =>
        {
            var connectionString = args[0];
            builder.UseSqlServer(connectionString);
        }).AddRepositories();
        
        var serviceProvider = services.BuildServiceProvider();
        
        var repository = serviceProvider.GetRequiredService<IProductRepository>();

        var productId = Guid.NewGuid();
        var result = await repository.AddProduct(0, new Product()
        {
            Id = productId,
            Name = $"Product {productId}",
            Description = "Product description",
            Price = 100,
        });

        if (result.IsSuccess)
        {
            return;
        }

        Console.WriteLine(result.Error);
    }
}