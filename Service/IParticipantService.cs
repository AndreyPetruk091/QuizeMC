using Domain.Entities;
using Domain.Exceptions;
using ValueObjects;

namespace Service
{
    public interface IParticipantService : IService<Participant>
    {
        /// <summary>
        /// Регистрация нового участника
        /// </summary>
        /// <param name="username">Объект Username (ValueObject)</param>
        Task<Participant> RegisterAsync(Username username, CancellationToken ct);

        /// <summary>
        /// Поиск участника по строковому username (без создания ValueObject)
        /// </summary>
        Task<Participant?> FindByUsernameAsync(string username, CancellationToken ct);

        /// <summary>
        /// Блокировка участника
        /// </summary>
        Task BanAsync(Guid participantId, string reason, CancellationToken ct);

        /// <summary>
        /// Обновление username участника
        /// </summary>
        /// <param name="newUsername">Объект Username (ValueObject)</param>
        Task UpdateUsernameAsync(Guid participantId, Username newUsername, CancellationToken ct);
    }
}