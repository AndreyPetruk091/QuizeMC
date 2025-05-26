// Infrastructure/EntityFramework/Configurations/ParticipantQuizConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizeMC.Domain.Entities;

public class ParticipantQuizConfiguration : IEntityTypeConfiguration<ParticipantQuiz>
{
    public void Configure(EntityTypeBuilder<ParticipantQuiz> builder)
    {
        builder.HasKey(pq => pq.Id);

        // Связь с Participant
        builder.HasOne(pq => pq.Participant)
            .WithMany(p => p.CompletedQuizzes)
            .HasForeignKey(pq => pq.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь с Quiz
        builder.HasOne(pq => pq.Quiz)
            .WithMany(q => q.Participants)
            .HasForeignKey(pq => pq.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}