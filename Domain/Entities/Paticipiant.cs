using Domain.Entities.Base;
using ValueObjects;
using ValueObjects.Exceptions;

namespace Domain.Entities
{
    public class Paticipiant : EntityBase
    {
        public Username Username { get; private set; }

        public Paticipiant (Username username)
        {
            Username = username ?? throw new UsernameException("Username cannot be null.");
        }

        public void UpdateUsername(Username newUsername)
        {
            Username = newUsername ?? throw new UsernameException("Username cannot be null.");
        }
    }
}
