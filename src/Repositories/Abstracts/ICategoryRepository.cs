using DatabaseContext.Entities;
using Repositories.Common;

namespace Repositories.Abstracts;

public interface ICategoryRepository
{
    Task<Result> AddCategoryAsync(int categoryId, string name);
    Task<Result> DeleteCategoryAsync(int categoryId);
    Task<List<Category>> GetAllCategoriesAsync();
}