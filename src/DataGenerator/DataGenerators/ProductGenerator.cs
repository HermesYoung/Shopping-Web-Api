using Bogus;
using DatabaseContext.Entities;
using Repositories.Abstracts;
using Repositories.Common;
using Repositories.Repositories.ProductRepository.Models;

namespace DataGenerator.DataGenerators;

public class ProductGenerator
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductGenerator(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> GenerateAsync(int count)
    {
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        var productDetailsFaker = new Faker<ProductDetail>()
            .RuleFor(product => product.Name, faker => faker.Commerce.ProductName())
            .RuleFor(product => product.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(product => product.Price, faker => faker.Random.Number(10, 1000))
            .RuleFor(product => product.IsDisabled, faker => faker.Random.Bool())
            .RuleFor(product => product.IsSoldOut, faker => faker.Random.Bool());

        var productFaker = new Faker<CategorizedProduct>()
            .RuleFor(product => product.CategoryId, faker => faker.PickRandom(categories.Select(x => x.Id)))
            .RuleFor(product => product.ProductDetail, () => productDetailsFaker);

        var categorizedProducts = productFaker.Generate(count);
        return await _productRepository.AddProductsAsync(categorizedProducts);
    }
}