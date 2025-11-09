using Microsoft.AspNetCore.Mvc;
using QuizeMC.Application.Models.Quiz;
using QuizeMC.Application.Models.Common;
using QuizeMC.Application.Services.Abstractions;

namespace QuizeMC.Presentation.WebHost.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminQuizzesController : ControllerBase
    {
        private readonly IQuizApplicationService _quizService;

        public AdminQuizzesController(IQuizApplicationService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuizzes([FromQuery] PagedRequest request)
        {
            var result = await _quizService.GetQuizzesPagedAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyQuizzes([FromQuery] PagedRequest request)
        {
            var adminId = GetCurrentAdminId();
            var result = await _quizService.GetQuizzesByAdminAsync(adminId, request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetQuiz(Guid id)
        {
            var result = await _quizService.GetQuizAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{id:guid}/details")]
        public async Task<IActionResult> GetQuizWithDetails(Guid id)
        {
            var result = await _quizService.GetQuizWithDetailsAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizCreateModel model)
        {
            var adminId = GetCurrentAdminId();
            var result = await _quizService.CreateQuizAsync(model, adminId);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetQuiz), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateQuiz(Guid id, [FromBody] QuizUpdateModel model)
        {
            var result = await _quizService.UpdateQuizAsync(id, model);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> UpdateQuizStatus(Guid id, [FromBody] QuizStatusUpdateModel model)
        {
            var result = await _quizService.UpdateQuizStatusAsync(id, model);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteQuiz(Guid id)
        {
            var result = await _quizService.DeleteQuizAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("{id:guid}/duplicate")]
        public async Task<IActionResult> DuplicateQuiz(Guid id)
        {
            var adminId = GetCurrentAdminId();
            var result = await _quizService.DuplicateQuizAsync(id, adminId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id:guid}/complexity")]
        public async Task<IActionResult> CalculateComplexity(Guid id)
        {
            var result = await _quizService.CalculateQuizComplexityAsync(id);

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