using DatabaseContext.Entities;
using Repositories.Common;
using Repositories.Repositories.CategoryRepository;
using Repositories.Repositories.CategoryRepository.Models;

namespace Repositories.Abstracts;

public interface ICategoryRepository
{
    Task<Result> AddCategoryAsync(int categoryId, string name);
    Task<Result> DeleteCategoryAsync(int categoryId);
    Task<List<CategoryDetail>> GetAllCategoriesAsync();
}