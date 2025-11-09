using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                });

#if DEBUG
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
#endif
            });

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
                    await context.Database.EnsureDeletedAsync();
                }

                // Применяем миграции
                await context.Database.MigrateAsync();

                // Проверяем подключение
                var canConnect = await context.Database.CanConnectAsync();
                if (!canConnect)
                {
                    throw new InvalidOperationException("Cannot connect to database after migration.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error during database initialization", ex);
            }
        }
    }
}