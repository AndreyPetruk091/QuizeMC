using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuizeMC.Application.Services;
using QuizeMC.Application.Services.Abstractions;
using QuizeMC.Application.Services.Mapping;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Domain.Services;
using QuizeMC.Domain.Services.Abstractions;
using QuizeMC.Infrastructure.EntityFramework;
using QuizeMC.Presentation.WebHost.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuizeMC Admin API", Version = "v1" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configuration
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Database Configuration
builder.Services.AddInfrastructure(builder.Configuration);

// Domain Services
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IQuizService, QuizService>();

// Application Services
builder.Services.AddScoped<IAdminApplicationService, AdminApplicationService>();
builder.Services.AddScoped<ICategoryApplicationService, CategoryApplicationService>();
builder.Services.AddScoped<IQuizApplicationService, QuizApplicationService>();
builder.Services.AddScoped<IQuestionApplicationService, QuestionApplicationService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(ApplicationProfile));

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
    };
});

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// HttpContext Accessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuizeMC Admin API v1");
        c.RoutePrefix = "swagger";
    });

    // Apply migrations and seed data in development
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Apply migrations
            await context.Database.MigrateAsync();
            Console.WriteLine("✅ Database migrations applied successfully!");

            // Seed test data with proper PasswordService
            await SeedTestDataWithPasswordService(scope.ServiceProvider);

            // Run health check
            await RunDatabaseHealthCheck(scope.ServiceProvider);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Failed to initialize database: {ex.Message}");
            throw;
        }
    }
}
else
{
    // In production, just apply migrations without seeding
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();
            Console.WriteLine("✅ Database migrations applied successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Failed to apply database migrations: {ex.Message}");
            // Don't throw in production - app might still work with existing database
        }
    }
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Custom Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Console.WriteLine("🚀 QuizeMC Admin API starting...");
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"URLs: {string.Join(", ", app.Urls)}");

app.Run();

// Helper method for seeding test data
async Task SeedTestDataWithPasswordService(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

    try
    {
        Console.WriteLine("🌱 Checking if test data needs to be seeded...");

        // Проверяем, есть ли уже администраторы в базе
        var existingAdmins = await unitOfWork.AdminRepository.GetAllAsync();
        if (!existingAdmins.Any())
        {
            Console.WriteLine("👤 Creating test admin with secure password...");

            // Создаем тестового администратора с правильным хешированием пароля
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
                ),
                new QuizeMC.Domain.Entities.Category(
                    new QuizeMC.Domain.ValueObjects.CategoryName("Science"),
                    new QuizeMC.Domain.ValueObjects.CategoryDescription("Scientific discoveries and phenomena"),
                    admin
                ),
                new QuizeMC.Domain.Entities.Category(
                    new QuizeMC.Domain.ValueObjects.CategoryName("Geography"),
                    new QuizeMC.Domain.ValueObjects.CategoryDescription("Countries, capitals, and geographical features"),
                    admin
                )
            };

            foreach (var category in categories)
            {
                await unitOfWork.CategoryRepository.AddAsync(category);
            }
            await unitOfWork.SaveChangesAsync();

            // Создаем тестовую викторину
            Console.WriteLine("❓ Creating test quiz...");

            var programmingCategory = categories.First();
            var quizTitle = new QuizeMC.Domain.ValueObjects.QuizTitle("C# Programming Basics");
            var quizDescription = new QuizeMC.Domain.ValueObjects.QuizDescription("Test your knowledge of C# programming fundamentals");

            var quiz = new QuizeMC.Domain.Entities.Quiz(quizTitle, quizDescription, programmingCategory, admin);

            // Создаем вопросы (пока без ответов и правильного индекса)
            var question1 = new QuizeMC.Domain.Entities.Question(
                new QuizeMC.Domain.ValueObjects.QuestionText("What keyword is used to define a class in C#?"),
                new QuizeMC.Domain.ValueObjects.AnswerIndex(0) // Временный индекс
            );

            var question2 = new QuizeMC.Domain.Entities.Question(
                new QuizeMC.Domain.ValueObjects.QuestionText("Which of the following is a value type in C#?"),
                new QuizeMC.Domain.ValueObjects.AnswerIndex(0) // Временный индекс
            );

            var question3 = new QuizeMC.Domain.Entities.Question(
                new QuizeMC.Domain.ValueObjects.QuestionText("What is the default access modifier for class members in C#?"),
                new QuizeMC.Domain.ValueObjects.AnswerIndex(0) // Временный индекс
            );

            // Добавляем ответы к первому вопросу
            var answersForQuestion1 = new[]
            {
                new QuizeMC.Domain.ValueObjects.Answer(new QuizeMC.Domain.ValueObjects.AnswerText("class")),
                new QuizeMC.Domain.ValueObjects.Answer(new QuizeMC.Domain.ValueObjects.AnswerText("struct")),
                new QuizeMC.Domain.ValueObjects.Answer(new QuizeMC.Domain.ValueObjects.AnswerText("interface")),
                new QuizeMC.Domain.ValueObjects.Answer(new QuizeMC.Domain.ValueObjects.AnswerText("object"))
            };
            question1.AddAnswers(answersForQuestion1);
            question1.UpdateCorrectAnswerIndex(new QuizeMC.Domain.ValueObjects.AnswerIndex(0)); // Правильный ответ: "class"

            // Добавляем ответы ко второму вопросу
            var answersForQuestion2 = new[]
            {
                new QuizeMC.Domain.ValueObjects.Answer(new QuizeMC.Domain.ValueObjects.AnswerText("string")),
                new QuizeMC.Domain.ValueObjects.Answer(new QuizeMC.Domain.ValueObjects.AnswerText("int")),
                new QuizeMC.Domain.ValueObjects.Answer(new QuizeMC.Domain.ValueObjects.AnswerText("List<int>")),
                new QuizeMC.Domain.ValueObjects.Answer(new QuizeMC.Domain.ValueObjects.AnswerText("object"))
            };
            question2.AddAnswers(answersForQuestion2);
            question2.UpdateCorrectAnswerIndex(new QuizeMC.Domain.ValueObjects.AnswerIndex(1)); // Правильный ответ: "int"

            // Добавляем ответы к третьему вопросу
            var answersForQuestion3 = new[]
            {
                new QuizeMC.Domain.ValueObjects.Answer(new QuizeMC.Domain.ValueObjects.AnswerText("public")),
                new QuizeMC.Domain.ValueObjects.Answer(new QuizeMC.Domain.ValueObjects.AnswerText("private")),
                new QuizeMC.Domain.ValueObjects.Answer(new QuizeMC.Domain.ValueObjects.AnswerText("internal")),
                new QuizeMC.Domain.ValueObjects.Answer(new QuizeMC.Domain.ValueObjects.AnswerText("protected"))
            };
            question3.AddAnswers(answersForQuestion3);
            question3.UpdateCorrectAnswerIndex(new QuizeMC.Domain.ValueObjects.AnswerIndex(1)); // Правильный ответ: "private"

            // Добавляем вопросы к викторине
            quiz.AddQuestion(question1);
            quiz.AddQuestion(question2);
            quiz.AddQuestion(question3);

            await unitOfWork.QuizRepository.AddAsync(quiz);
            await unitOfWork.SaveChangesAsync();

            Console.WriteLine("✅ Test data seeded successfully!");
            Console.WriteLine("   - Admin: admin@quizemc.com / Admin123!");
            Console.WriteLine("   - 5 categories created");
            Console.WriteLine("   - 1 quiz with 3 questions created");
        }
        else
        {
            Console.WriteLine("ℹ️ Test data already exists, skipping seeding.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error seeding test data: {ex.Message}");
        // Don't throw - app can still work without test data
    }
}

// Helper method for database health check
async Task RunDatabaseHealthCheck(IServiceProvider serviceProvider)
{
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
        // Don't throw - health check failure shouldn't stop the app
    }
}