using QuizeMC.Domain.Exceptions;
using QuizeMC.Domain.Entities.Base;
using QuizeMC.Domain.ValueObjects;
using System.Collections.Generic;

namespace QuizeMC.Domain.Entities
{
    public class Participant : EntityBase
    {
        public Username Username { get; private set; }
        public bool IsBanned { get; private set; }

        // Навигационное свойство для связи многие-ко-многим с Quiz
        public virtual ICollection<ParticipantQuiz> CompletedQuizzes { get; private set; } = new List<ParticipantQuiz>();

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

        // Метод для добавления пройденного квиза
        public void AddCompletedQuiz(Quiz quiz, DateTime completionTime)
        {
            CompletedQuizzes.Add(new ParticipantQuiz
            {
                Participant = this,
                Quiz = quiz,
                CompletedAt = completionTime
            });
        }
    }
}