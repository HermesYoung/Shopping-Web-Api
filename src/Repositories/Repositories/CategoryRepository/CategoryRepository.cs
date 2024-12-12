using DatabaseContext.Context;
using DatabaseContext.Entities;
using Repositories.Abstracts;
using Repositories.Common;

namespace Repositories.Repositories.CategoryRepository;

internal class CategoryRepository : ICategoryRepository
{
    private readonly ShoppingWebDbContext _shoppingWebDbContext;

    public CategoryRepository(ShoppingWebDbContext shoppingWebDbContext)
    {
        _shoppingWebDbContext = shoppingWebDbContext;
    }

    public async Task<Result> AddCategoryAsync(int categoryId, int? parentId, string name)
    {
        var category = _shoppingWebDbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        if (category != null) return Result.Failure();
        _shoppingWebDbContext.Categories.Add(new Category { Name = name , Id = categoryId , Parent = parentId });
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> MoveSubcategoryAsync(int categoryId, int parentId)
    {
        var category = _shoppingWebDbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        var parent = _shoppingWebDbContext.Categories.FirstOrDefault(c => c.Id == parentId);

        if (category == null) return Result.Failure();
        if (parent == null) return Result.Failure();

        category.Parent = parentId;
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> MoveCategoryToRootAsync(int categoryId)
    {
        var category = _shoppingWebDbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        if (category == null) return Result.Failure();
        category.Parent = null;
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteCategoryAsync(int categoryId)
    {
        var category = _shoppingWebDbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        if (category == null) return Result.Failure();
        if (_shoppingWebDbContext.Categories.Any(x => x.Parent.HasValue && x.Parent.Value == categoryId))
            return Result.Failure();

        _shoppingWebDbContext.Categories.Remove(category);
        await _shoppingWebDbContext.SaveChangesAsync();
        return Result.Success();
    }
}