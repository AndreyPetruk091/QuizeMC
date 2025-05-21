using Microsoft.EntityFrameworkCore;
using QuizeMC.Infrastructure.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Регистрация DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление контроллеров
builder.Services.AddControllers();



var app = builder.Build();




app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();