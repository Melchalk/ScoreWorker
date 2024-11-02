using ScoreWorker.Models.Db;
using ScoreWorker.Models.DTO;
using System.Text.Json;

namespace ScoreWorkerDB;

public static class DbHelper
{
    private const string file = "sample_reviews.json";

    public static async Task UpdateDatabase()
    {
        string jsonString = await File.ReadAllTextAsync(file);

        var reviews = JsonSerializer.Deserialize<List<ReviewInfo>>(jsonString)!
            .Select(r => new DbReview()
            {
                Id = Guid.NewGuid(),
                IDReviewer = r.IDReviewer,
                IDUnderReview = r.IDUnderReview,
                Review = r.Review,
            });


    }
}
