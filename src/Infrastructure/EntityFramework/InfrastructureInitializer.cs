using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Domain.Services.Abstractions;
using QuizeMC.Infrastructure.EntityFramework.Repositories;

namespace QuizeMC.Infrastructure.EntityFramework
{
    public static class InfrastructureInitializer
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            // Регистрируем конфигурацию базы данных
            services.Configure<DatabaseConfig>(configuration.GetSection("Database"));

            // Регистрируем DbContext
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var databaseConfig = serviceProvider.GetService<IOptions<DatabaseConfig>>()?.Value;
                string connectionString;

                if (databaseConfig != null)
                {
                    connectionString = databaseConfig.GetConnectionString();
                }
                else
                {
                    connectionString = configuration.GetConnectionString("DefaultConnection");

                    if (string.IsNullOrEmpty(connectionString))
                    {
                        throw new InvalidOperationException(
                            "Could not find database configuration. " +
                            "Please configure either 'Database' section or 'ConnectionStrings:DefaultConnection' in appsettings.json");
                    }
                }

                Console.WriteLine($"🔗 Configuring DbContext with connection string: {connectionString}");

                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                });

                // Включаем подробные ошибки только в Development
#if DEBUG
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
#endif
            });

            // Регистрируем репозитории
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            Console.WriteLine("✅ Infrastructure services registered successfully.");

            return services;
        }

        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider, bool ensureDeleted = false)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                if (ensureDeleted)
                {
                    Console.WriteLine("🗑️ Deleting database...");
                    await context.Database.EnsureDeletedAsync();
                }

                Console.WriteLine("🔄 Applying migrations...");

                // Получаем pending миграции
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    Console.WriteLine($"📋 Pending migrations: {string.Join(", ", pendingMigrations)}");
                    await context.Database.MigrateAsync();
                    Console.WriteLine("✅ Migrations applied successfully.");
                }
                else
                {
                    Console.WriteLine("ℹ️ No pending migrations.");
                }

                // Проверяем, что база создана и доступна
                var canConnect = await context.Database.CanConnectAsync();
                if (canConnect)
                {
                    Console.WriteLine("🔗 Database connection test: SUCCESS");

                    // Проверяем существование основных таблиц
                    await CheckDatabaseTables(context);
                }
                else
                {
                    Console.WriteLine("❌ Database connection test: FAILED");
                    throw new InvalidOperationException("Cannot connect to database after migration.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error during database initialization: {ex.Message}");
                throw;
            }
        }

        public static async Task SeedTestDataAsync(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            using var scope = serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

            try
            {
                Console.WriteLine("🌱 Seeding test data...");

                // Проверяем, есть ли уже администраторы в базе
                var existingAdmins = await unitOfWork.AdminRepository.GetAllAsync();
                if (!existingAdmins.Any())
                {
                    Console.WriteLine("👤 Creating test admin...");

                    // Создаем тестового администратора
                    var adminEmail = new QuizeMC.Domain.ValueObjects.Email("admin@quizemc.com");
                    var passwordHash = passwordService.HashPassword("Admin123!");

                    var admin = new QuizeMC.Domain.Entities.Admin(adminEmail, passwordHash);

                    await unitOfWork.AdminRepository.AddAsync(admin);
                    await unitOfWork.SaveChangesAsync();

                    // Создаем тестовые категории
                    Console.WriteLine("📚 Creating test categories...");

                    var categories = new[]
                    {
                        new QuizeMC.Domain.Entities.Category(
                            new QuizeMC.Domain.ValueObjects.CategoryName("Programming"),
                            new QuizeMC.Domain.ValueObjects.CategoryDescription("Questions about programming languages, algorithms and technologies"),
                            admin
                        ),
                        new QuizeMC.Domain.Entities.Category(
                            new QuizeMC.Domain.ValueObjects.CategoryName("Mathematics"),
                            new QuizeMC.Domain.ValueObjects.CategoryDescription("Mathematical problems and puzzles"),
                            admin
                        ),
                        new QuizeMC.Domain.Entities.Category(
                            new QuizeMC.Domain.ValueObjects.CategoryName("History"),
                            new QuizeMC.Domain.ValueObjects.CategoryDescription("Historical events and personalities"),
                            admin
                        )
                    };

                    foreach (var category in categories)
                    {
                        await unitOfWork.CategoryRepository.AddAsync(category);
                    }
                    await unitOfWork.SaveChangesAsync();

                    Console.WriteLine("✅ Test data seeded successfully!");
                }
                else
                {
                    Console.WriteLine("ℹ️ Test data already exists, skipping seeding.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error seeding test data: {ex.Message}");
                throw;
            }
        }

        private static async Task CheckDatabaseTables(ApplicationDbContext context)
        {
            try
            {
                var tables = new[] { "Admins", "Categories", "Quizzes", "Questions" };
                var existingTables = new List<string>();

                foreach (var table in tables)
                {
                    var tableExists = await context.Database.SqlQueryRaw<int>(
                        $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'public' AND table_name = '{table.ToLower()}'")
                        .FirstOrDefaultAsync() > 0;

                    if (tableExists)
                    {
                        existingTables.Add(table);
                    }
                }

                if (existingTables.Count == tables.Length)
                {
                    Console.WriteLine($"✅ All tables created: {string.Join(", ", existingTables)}");
                }
                else
                {
                    Console.WriteLine($"⚠️ Some tables missing. Created: {string.Join(", ", existingTables)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Could not check table existence: {ex.Message}");
            }
        }

        public static async Task RunDatabaseHealthCheckAsync(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                Console.WriteLine("🏥 Running database health check...");

                // Проверка подключения
                var canConnect = await context.Database.CanConnectAsync();
                Console.WriteLine($"🔗 Database connection: {(canConnect ? "✅ HEALTHY" : "❌ UNHEALTHY")}");

                if (canConnect)
                {
                    // Проверка примененных миграций
                    var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
                    Console.WriteLine($"📋 Applied migrations: {appliedMigrations.Count()}");

                    // Проверка счетчиков записей
                    var adminCount = await context.Admins.CountAsync();
                    var categoryCount = await context.Categories.CountAsync();
                    var quizCount = await context.Quizzes.CountAsync();
                    var questionCount = await context.Questions.CountAsync();

                    Console.WriteLine($"📊 Database statistics:");
                    Console.WriteLine($"   👤 Admins: {adminCount}");
                    Console.WriteLine($"   📚 Categories: {categoryCount}");
                    Console.WriteLine($"   ❓ Quizzes: {quizCount}");
                    Console.WriteLine($"   ⁉️ Questions: {questionCount}");

                    Console.WriteLine("✅ Database health check completed successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Database health check failed: {ex.Message}");
                throw;
            }
        }
    }
}