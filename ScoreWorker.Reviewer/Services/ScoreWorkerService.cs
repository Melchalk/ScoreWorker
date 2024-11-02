using Refit;
using SampleSolution.Services.Interfaces;
using ScoreWorker.Models.DTO;
using ScoreWorker.RefitApi;
using System.Text;
using System.Text.Json;

namespace SampleSolution.Services;

public class ScoreWorkerService : IScoreWorkerService
{
    private const string file = "review_dataset.json";

    public async Task<string> GetResponse(int id)
    {
        var allReviews = await LoadReviews();

        var reviews = allReviews.Where(r => r.IDUnderReview == id).ToList();

        var prompt = PreparePrompt(reviews);

        return await EvaluateReviewsWithLLM(prompt);
    }

    public async Task<List<ReviewInfo>?> LoadReviews()
    {
        string jsonString = await File.ReadAllTextAsync(file);

        return JsonSerializer.Deserialize<List<ReviewInfo>>(jsonString);
    }

    public string PreparePrompt(List<ReviewInfo> reviews)
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("Here are some reviews about an employee:");
        for (int i = 0; i < reviews.Count; i++)
            builder.AppendLine($"Review {i}:\n{reviews[i].Review}");

        builder.AppendLine("Based on these reviews, evaluate the employee on a scale from 1 to 5 for the following criteria:");
        builder.AppendLine("1. Professionalism\n2. Teamwork\n3. Communication\n4. Initiative\n5. Overall Performance");
        builder.AppendLine("Add short (5 sentences) explanation for each score you assigned.");

        return builder.ToString();
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

        return await apiService.GetProductsList(request);
    }
}
