using Microsoft.EntityFrameworkCore;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Infrastructure.EntityFramework;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Participant> Participants => Set<Participant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурация для Quiz
        modelBuilder.Entity<Quiz>(q =>
        {
            q.Property(x => x.Title)
                .HasConversion(t => t.Value, v => new QuizTitle(v));

            q.HasMany(x => x.Questions)
                .WithOne()
                .HasForeignKey("QuizId");
        });

        // Конфигурация для Question
        modelBuilder.Entity<Question>(q =>
        {
            q.Property(x => x.Text)
                .HasConversion(t => t.Value, v => new QuestionText(v));

            q.Property(x => x.CorrectAnswerIndex)
                .HasConversion(i => i.Value, v => new AnswerIndex(v));

            // Настройка Answers как owned types (без ключа)
            q.OwnsMany(x => x.Answers, a =>
            {
                a.Property(ans => ans.Text)
                    .HasConversion(t => t.Value, v => new AnswerText(v));
            });
        });

        // Конфигурация для Participant
        modelBuilder.Entity<Participant>(p =>
        {
            p.Property(x => x.Username)
                .HasConversion(u => u.Value, v => new Username(v));
        });

        base.OnModelCreating(modelBuilder);
    }
} 