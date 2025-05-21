using QuizeMC.Domain.Exceptions;
using QuizeMC.Domain.Entities.Base;
using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Domain.Entities
{
    public class Participant : EntityBase
    {
        public Username Username { get; private set; }
        public bool IsBanned { get; private set; }

        // Конструктор для EF Core
        protected Participant() { }

        public Participant(Username username)
        {
            Username = username ?? throw new UsernameException("Username cannot be null.");
            IsBanned = false;
        }

        public void Ban(string reason)
        {
            if (IsBanned)
                throw new DomainException($"Participant {Id} is already banned.");

            IsBanned = true;
        }

        public void UpdateUsername(Username newUsername)
        {
            if (IsBanned)
                throw new DomainException("Banned participants cannot change username.");

            Username = newUsername ?? throw new UsernameException("Username cannot be null.");
        }
    }
}