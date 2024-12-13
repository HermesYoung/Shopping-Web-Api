using ManagementSite.Controllers.Models.Category;
using Microsoft.AspNetCore.Mvc;
using Repositories.Abstracts;

namespace ManagementSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryCreateBody body)
        {
            var result = await _categoryRepository.AddCategoryAsync(body.CategoryId, body.Name);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            
            return BadRequest(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            
            return Ok(categories.Select(category => new CategoryView(category.Id, category.Name)));
        }
    }
}