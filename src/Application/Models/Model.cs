namespace QuizeMC.Application.Application.Models
{
    /// <summary>
    /// Базовая модель участника системы
    /// </summary>
    /// <param name="Id">Уникальный идентификатор участника</param>
    /// <param name="Username">Имя пользователя</param>
    public abstract record Model(Guid Id, string Username)
        : IModel<Guid>;
}

