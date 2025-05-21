
using QuizeMC.Common.Enums;
using QuizeMC.Application.Models.Quiz;

namespace Services.Abstactions
{
    public interface IQuizResponseService
    {
        /// <summary>
        /// Отправка ответа на вопрос викторины
        /// </summary>
        /// <param name="response">Модель ответа</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Статус обработки ответа</returns>
        Task<QuizResponseStatus> SubmitResponseAsync(
            SubmitResponseModel response,
            CancellationToken cancellationToken = default);
    }
}