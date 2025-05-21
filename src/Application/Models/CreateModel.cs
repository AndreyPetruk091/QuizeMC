namespace QuizeMC.Application.Application.Models
{
    /// <summary>
    /// Базовая модель для создания участников системы (Participant, Admin и др.)
    /// Наследуется конкретными моделями создания (например, CreateParticipantModel)
    /// </summary>
    /// <param name="Id">Уникальный идентификатор участника</param>
    /// <param name="Username">Имя пользователя</param>
    public abstract record CreateModel(
        Guid Id,
        string Username)
        : ICreateModel;
}
