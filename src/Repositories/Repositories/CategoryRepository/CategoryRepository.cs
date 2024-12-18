using DatabaseContext.Context;
using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstracts;
using Repositories.Common;
using Repositories.Repositories.CategoryRepository.Models;

namespace Repositories.Repositories.CategoryRepository;

internal class CategoryRepository(ShoppingWebDbContext shoppingWebDbContext) : ICategoryRepository
{
    public async Task<Result> AddCategoryAsync(string name)
    {
        shoppingWebDbContext.Categories.Add(new Category { Name = name, Id = Guid.NewGuid() });
        await shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AddCategoriesAsync(IEnumerable<string> names)
    {
        var categories = names.Select(name => new Category { Name = name, Id = Guid.NewGuid() }).ToList();
        shoppingWebDbContext.Categories.AddRange(categories);
        await shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteCategoryAsync(Guid categoryId)
    {
        var category = shoppingWebDbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        if (category == null)
            return Result.Failure(Error.Create("Category not exists",
                new ErrorMessage(ErrorCode.CategoryNotExists, new { CategoryId = categoryId })));

        shoppingWebDbContext.Categories.Remove(category);
        await shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<List<CategoryDetail>> GetAllCategoriesAsync()
    {
        var categories = await shoppingWebDbContext.Categories.ToListAsync();
        return categories.Select(category => new CategoryDetail(category.Id, category.Name)).ToList();
    }
}