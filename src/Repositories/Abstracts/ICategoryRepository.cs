using DatabaseContext.Entities;
using Repositories.Common;
using Repositories.Repositories.CategoryRepository;
using Repositories.Repositories.CategoryRepository.Models;

namespace Repositories.Abstracts;

public interface ICategoryRepository
{
    Task<Result> AddCategoryAsync(string name);
    Task<Result> DeleteCategoryAsync(Guid categoryId);
    Task<List<CategoryDetail>> GetAllCategoriesAsync();
    Task<Result> AddCategoriesAsync(IEnumerable<string> names);
}