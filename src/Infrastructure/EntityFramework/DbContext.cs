using Microsoft.EntityFrameworkCore;
using QuizeMC.Domain.Entities;

namespace QuizeMC.Infrastructure.EntityFramework
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Quiz> Quizzes => Set<Quiz>();
        public DbSet<Question> Questions => Set<Question>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Базовая конфигурация без сложных индексов
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.OwnsOne(e => e.Email);
                entity.OwnsOne(e => e.PasswordHash);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.OwnsOne(e => e.Name);
                entity.OwnsOne(e => e.Description);
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.OwnsOne(e => e.Title);
                entity.OwnsOne(e => e.Description);
                entity.Property(e => e.Status).HasConversion<string>();
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.OwnsOne(e => e.Text);
                entity.OwnsOne(e => e.CorrectAnswerIndex);

                entity.OwnsMany(e => e.Answers, a =>
                {
                    a.WithOwner().HasForeignKey("QuestionId");
                    a.Property<int>("Id").ValueGeneratedOnAdd();
                    a.HasKey("Id");
                    a.OwnsOne(ans => ans.Text);
                });
            });
        }
    }
}