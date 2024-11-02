using ScoreWorkerDB;
using System.Text.Json;
/*
class Program
{
    static async Task Main(string[] args)
    {
        // Загружаем данные из JSON-файла
        var jsonData = await File.ReadAllTextAsync("reviews.json");
        var reviews = JsonSerializer.Deserialize<List<DbReview>>(jsonData);

        // Создаем базу данных и сохраняем данные
        using (var context = new ReviewContext())
        {
            // Создаем базу данных, если она еще не создана
            await context.Database.EnsureCreatedAsync();

            // Добавляем отзывы и сохраняем изменения
            context.Reviews.AddRange(reviews);
            await context.SaveChangesAsync();

            Console.WriteLine("Данные успешно сохранены в базе данных.");
        }
    }
}*/
