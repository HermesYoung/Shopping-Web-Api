using Microsoft.Extensions.DependencyInjection;
using Repositories.Abstracts;
using Repositories.Repositories.ProductRepository;

namespace Repositories.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IProductRepository, ProductRepository>();
        return services;
    }
}