using NBomber.Contracts;
using NBomber.CSharp;

// 1. Создаем общий HTTP-клиент
using var client = new HttpClient();

// Базовый URL нашего API, запущенного локально в Docker
const string baseUrl = "http://localhost:8080";

// СЦЕНАРИЙ 1: Линейная нагрузка
var linearGrowthScenario = Scenario.Create("linear_growth_scenario", async context =>
{
    try
    {
        var response = await client.GetAsync(baseUrl + "/");

        return response.IsSuccessStatusCode
            ? Response.Ok()
            : Response.Fail();
    }
    catch (Exception)
    {
        return Response.Fail();
    }
})
.WithoutWarmUp() // ОТКЛЮЧАЕМ ПРОГРЕВ
.WithLoadSimulations(
    Simulation.KeepConstant(copies: 10, during: TimeSpan.FromSeconds(30))
);

// СЦЕНАРИЙ 2: Пиковая нагрузка (Стресс-тест)
var peakLoadScenario = Scenario.Create("peak_load_scenario", async context =>
{
    try
    {
        var response = await client.GetAsync(baseUrl + "/");

        return response.IsSuccessStatusCode
            ? Response.Ok()
            : Response.Fail();
    }
    catch (Exception)
    {
        return Response.Fail();
    }
})
.WithoutWarmUp() // ОТКЛЮЧАЕМ ПРОГРЕВ
.WithLoadSimulations(
    Simulation.KeepConstant(copies: 20, during: TimeSpan.FromSeconds(15))
);

// 2. Регистрируем и запускаем оба сценария нагрузки по очереди
NBomberRunner
    .RegisterScenarios(linearGrowthScenario, peakLoadScenario)
    .Run();

Console.WriteLine("Нагрузочное тестирование успешно завершено!");
