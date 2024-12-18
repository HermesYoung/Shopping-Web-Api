using Bogus;
using DatabaseContext.Entities;
using Repositories.Abstracts;
using Repositories.Common;

namespace DataGenerator.DataGenerators;

public class CategoryDataGenerator
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryDataGenerator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<Result> GenerateAsync(int num)
    {
        var categories = new List<string>();
        while (categories.Count < num)
        {
            var generatedCategories = new Faker().Commerce.Categories(num).Distinct();
            foreach (var generatedCategory in generatedCategories)
            {
                if (categories.Contains(generatedCategory))
                {
                    continue;
                }
                categories.Add(generatedCategory);
                if (categories.Count >= num)
                {
                    break;
                }
            }
        }
        
        return _categoryRepository.AddCategoriesAsync(categories);
    }
}