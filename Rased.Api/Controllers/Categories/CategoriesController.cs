using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Categories;
using Rased.Business.Services.Categories;

namespace Rased.Api.Controllers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("Create", Name = "CreateCategory")]
        public async Task<IActionResult> CreateNewCategory(CategoryDto model)
        {
            var result = await _categoryService.CreateNewCategory(model);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("Update/{id:int}", Name = "UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDto model)
        {
            var result = await _categoryService.UpdateCategory(id, model);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}", Name = "DeleteCategory")]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            var result = await _categoryService.RemoveCategory(id);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("All", Name = "AllCategory")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllCategories();

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("Single/{id:int}", Name = "SingleCategory")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.GetCategoryById(id);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
