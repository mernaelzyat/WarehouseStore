using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseStores.Core.Dto.CategoryDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Core.Models;

namespace WarehouseStores.Api.Controllers
{

    public class CategoryController : BaseApiController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("AllCategories")] // GET: api/categories
        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return categories;
        }

        [HttpGet("CategoryById")] // GET: api/categories/id
        public async Task<ActionResult> GetCategoryById(int id, int pageNumber = 1, int pageSize = 10)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id, pageNumber, pageSize);
            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(SimpleCategoryDto simpleCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addedCategory = await _categoryRepository.AddCategoryAsync(simpleCategoryDto);

            return Ok(addedCategory);
        }
        [HttpPut("{id}")]  // PUT: api/categories/id
        public async Task<ActionResult> UpdateCategory(int id, SimpleCategoryDto category)
        {
            if(id != category.Id) return BadRequest(ModelState);

            await _categoryRepository.UpdateCategoryAsync(category);
            return Ok(category);

        }
        // DELETE: api/categories/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryRepository.DeleteCategoryAsync(id);

         

            return Ok("Category Deleted");



           // return NoContent();
        }

    }
}
