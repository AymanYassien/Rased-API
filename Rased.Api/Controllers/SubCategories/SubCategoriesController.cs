using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.SubCategories;
using Rased.Business.Services.SubCategories;

namespace Rased.Api.Controllers.SubCategories
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SubCategoriesController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoriesController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpPost("Create", Name = "CreateSubcategory")]
        public async Task<IActionResult> CreateNewSubCategory(CreateSubCategoryDto model)
        {
            var result = await _subCategoryService.CreateNewSubCategory(model)
                ;
            if (!result.Succeeded)
                return BadRequest(result);
            
            return Ok(result);
        }

        [HttpPut("Update/{id:int}", Name = "UpdateSubcategory")]
        public async Task<IActionResult> UpdateSubCategory(int id, CreateSubCategoryDto model)
        {
            var result = await _subCategoryService.UpdateSubCategory(id, model);

            if (!result.Succeeded)
                return BadRequest(result);
            
            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}", Name = "DeleteSubcategory")]
        public async Task<IActionResult> RemoveSubCategory(int id)
        {
            var result = await _subCategoryService.RemoveSubCategory(id);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("Single/{id:int}", Name = "SingleSubcategory")]
        public async Task<IActionResult> GetSingleSubCategory(int id)
        {
            var result = await _subCategoryService.GetSubCategoryById(id);

            if (!result.Succeeded)
                return BadRequest(result);
            
            return Ok(result);
        }
    }
}
