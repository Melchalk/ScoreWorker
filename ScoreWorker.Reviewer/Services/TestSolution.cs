using Refit;
using ScoreWorker.Domain.Services.Interfaces;
using ScoreWorker.Models.DTO;
using ScoreWorker.RefitApi;
using System.Text;
using System.Text.Json;

namespace ScoreWorker.Domain.Services;

public class TestSolution : ITestSolution
{
    private const string file = "sample_reviews.json";
    private const string filePrompt = "prompt.txt";

    public async Task<string> GetResponse()
    {
        var reviews = await LoadReviews();

        var prompt = await PreparePrompt(reviews);

        return await EvaluateReviewsWithLLM(prompt);
    }

    public async Task<List<ReviewInfo>?> LoadReviews()
    {
        string jsonString = await File.ReadAllTextAsync(file);

        return JsonSerializer.Deserialize<List<ReviewInfo>>(jsonString);
    }

    private async Task<string> PreparePrompt(List<ReviewInfo> reviews)
    {
        StringBuilder builder = new();

        for (int i = 1; i <= reviews.Count; i++)
            builder.AppendLine($"Review {i}:\n{reviews[i-1].Review}");

        string jsonString = await File.ReadAllTextAsync(filePrompt);

        return string.Format(jsonString, builder.ToString());
    }

    public async Task<string> EvaluateReviewsWithLLM(string prompt)
    {
        var apiService = RestService.For<IVkControllerApi>(IVkControllerApi.VkScoreWorkerApi);

        var request = new GenerateScoreRequest()
        {
            Prompt = prompt,
            ApplyChatTemplate = true,
            SystemPrompt = "You are a helpful assistant.",
            MaxTokens = 400,
            N = 1,
            Temperature = 0.7
        };

        return await apiService.GenerateScore(request);
    }
}
