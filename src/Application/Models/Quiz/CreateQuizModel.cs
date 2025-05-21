using QuizeMC.Application.Models.Question;

namespace QuizeMC.Application.Application.Models.Quiz
{
    public class CreateQuizModel
    {
        public string Title { get; set; }
        public List<QuestionCreateModel> Questions { get; set; }
    }
}