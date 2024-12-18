using DatabaseContext.Context;
using DatabaseContext.Entities;
using DataGenerator.DataGenerators;
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
        services.AddDbContext(args[0]).AddRepositories()
            .AddSingleton<CategoryDataGenerator>()
            .AddSingleton<ProductGenerator>();

        var serviceProvider = services.BuildServiceProvider();
        
        var categoryDataGenerator = serviceProvider.GetRequiredService<CategoryDataGenerator>();
        var productGenerator = serviceProvider.GetRequiredService<ProductGenerator>();
        Console.WriteLine("Generate categories");
        var categoriesGenerateResult = await categoryDataGenerator.GenerateAsync(10);
        if (!categoriesGenerateResult.IsSuccess)
        {
            Console.WriteLine("Failed to create categories");
            return;
        }

        Console.WriteLine("Finished generating categories");
        
        Console.WriteLine("Generate products");
        var productsGenerateResult = await productGenerator.GenerateAsync(100);
        if (!productsGenerateResult.IsSuccess)
        {
            Console.WriteLine("Failed to create products");
            return;
        }

        Console.WriteLine("Finished generating products");
        
        Console.WriteLine("Data generated");
    }
}