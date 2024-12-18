using DatabaseContext.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Abstracts;
using Repositories.Repositories.CategoryRepository;
using Repositories.Repositories.OrderRepository;
using Repositories.Repositories.ProductRepository;
using Repositories.Repositories.PromotionRepository;

namespace Repositories.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IPromotionRepository, PromotionRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ShoppingWebDbContext>(builder =>
        {
            builder.UseSqlServer(connectionString);
        });

        return services;
    }
}