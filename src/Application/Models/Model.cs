using QuizeMC.Application.Application.Models;

namespace QuizeMC.Application.Models
{
    /// <summary>
    /// Базовая модель сущности системы
    /// </summary>
    /// <param name="Id">Уникальный идентификатор сущности</param>
    public abstract record Model(Guid Id) : IModel<Guid>;
}
