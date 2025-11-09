using Microsoft.AspNetCore.Mvc;
using QuizeMC.Application.Models.Category;
using QuizeMC.Application.Models.Common;
using QuizeMC.Application.Services.Abstractions;

namespace QuizeMC.Presentation.WebHost.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminCategoriesController : ControllerBase
    {
        private readonly ICategoryApplicationService _categoryService;

        public AdminCategoriesController(ICategoryApplicationService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] PagedRequest request)
        {
            var result = await _categoryService.GetCategoriesPagedAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var result = await _categoryService.GetCategoryAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateModel model)
        {
            var adminId = GetCurrentAdminId();
            var result = await _categoryService.CreateCategoryAsync(model, adminId);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetCategory), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryUpdateModel model)
        {
            var result = await _categoryService.UpdateCategoryAsync(id, model);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyCategories()
        {
            var adminId = GetCurrentAdminId();
            var result = await _categoryService.GetCategoriesByAdminAsync(adminId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        private Guid GetCurrentAdminId()
        {
            // Temporary implementation
            return Guid.Parse("00000000-0000-0000-0000-000000000001");
        }
    }
}