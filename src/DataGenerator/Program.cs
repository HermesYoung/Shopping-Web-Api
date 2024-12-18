using DatabaseContext.Context;
using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Abstracts;
using Repositories.Extensions;
using Repositories.Repositories.ProductRepository;
using Repositories.Repositories.ProductRepository.Models;

namespace DataGenerator;

public class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddDbContext(args[0]).AddRepositories();

        var serviceProvider = services.BuildServiceProvider();

        var repository = serviceProvider.GetRequiredService<IProductRepository>();

        var categorizedProducts = new List<CategorizedProduct>
            { new(Guid.NewGuid(), new ProductDetail("name", "description", 100, false, false)) };
        var result = await repository.AddProductsAsync(categorizedProducts);
        
        if (!result.IsSuccess)
        {
            Console.WriteLine(result.Error);
        }
    }
}