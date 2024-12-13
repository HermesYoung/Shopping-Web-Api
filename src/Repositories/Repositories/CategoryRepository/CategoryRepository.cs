using DatabaseContext.Context;
using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstracts;
using Repositories.Common;
using Repositories.Common.Errors;

namespace Repositories.Repositories.CategoryRepository;

internal class CategoryRepository(ShoppingWebDbContext shoppingWebDbContext) : ICategoryRepository
{
    public async Task<Result> AddCategoryAsync(int categoryId, string name)
    {
        var category = shoppingWebDbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        if (category != null)
            return Result.Failure(CategoryRepositoryError.CreateCategoryAlreadyExistError(categoryId));
        shoppingWebDbContext.Categories.Add(new Category { Name = name, Id = categoryId });
        await shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteCategoryAsync(int categoryId)
    {
        var category = shoppingWebDbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        if (category == null) return Result.Failure(CategoryRepositoryError.CreateCategoryNotFoundError(categoryId));
        
        shoppingWebDbContext.Categories.Remove(category);
        await shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await shoppingWebDbContext.Categories.ToListAsync();
    }
}