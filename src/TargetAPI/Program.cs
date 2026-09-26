using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

// Настраиваем Serilog для вывода структурированного JSON в консоль Docker
// Настраиваем Serilog для вывода структурированного JSON в консоль Docker
    Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .CreateLogger();


builder.Host.UseSerilog();

var app = builder.Build();

// Простой эндпоинт, который мы будем нагружать
app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("Получен запрос на тестовый эндпоинт нагрузки");
    return "API для тестирования нагрузки";
});

app.Run();
