
using QuizeMC.Domain.Entities.Base;
using QuizeMC.Domain.Entities;

public class ParticipantQuiz : EntityBase
{
    public Guid ParticipantId { get; set; }
    public Guid QuizId { get; set; }
    public DateTime CompletedAt { get; set; }

    public virtual Participant Participant { get; set; }
    public virtual Quiz Quiz { get; set; }
}