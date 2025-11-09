using Microsoft.AspNetCore.Mvc;
using QuizeMC.Application.Models.Question;
using QuizeMC.Application.Services.Abstractions;

namespace QuizeMC.Presentation.WebHost.Controllers
{
    [ApiController]
    [Route("api/admin/quizzes/{quizId:guid}/[controller]")]
    public class AdminQuestionsController : ControllerBase
    {
        private readonly IQuestionApplicationService _questionService;

        public AdminQuestionsController(IQuestionApplicationService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions(Guid quizId)
        {
            var result = await _questionService.GetQuestionsByQuizAsync(quizId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetQuestion(Guid quizId, Guid id)
        {
            var result = await _questionService.GetQuestionAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(Guid quizId, [FromBody] QuestionCreateModel model)
        {
            var result = await _questionService.CreateQuestionAsync(model, quizId);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetQuestion), new { quizId, id = result.Data?.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateQuestion(Guid quizId, Guid id, [FromBody] QuestionUpdateModel model)
        {
            var result = await _questionService.UpdateQuestionAsync(id, model);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteQuestion(Guid quizId, Guid id)
        {
            var result = await _questionService.DeleteQuestionAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("reorder")]
        public async Task<IActionResult> ReorderQuestions(Guid quizId, [FromBody] List<Guid> questionIds)
        {
            var result = await _questionService.ReorderQuestionsAsync(quizId, questionIds);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}