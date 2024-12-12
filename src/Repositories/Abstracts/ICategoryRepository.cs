using Repositories.Common;

namespace Repositories.Abstracts;

public interface ICategoryRepository
{
    Task<Result> AddCategoryAsync(int categoryId, int? parentId, string name);
    Task<Result> MoveSubcategoryAsync(int categoryId, int parentId);
    Task<Result> MoveCategoryToRootAsync(int categoryId);
    Task<Result> DeleteCategoryAsync(int categoryId);
}