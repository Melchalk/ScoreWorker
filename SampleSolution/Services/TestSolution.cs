using Refit;
using SampleSolution.Services.Interfaces;
using ScoreWorker.Models.DTO;
using ScoreWorker.RefitApi;
using System.Text.Json;

namespace SampleSolution.Services;

public class TestSolution : ITestSolution
{
    private const string file = "sample_reviews.json";

    public async Task<string> GetResponse()
    {
        var reviews = await LoadReviews();

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
        string prompt = "Here are some reviews about an employee:\n\n";
        for (int i = 0; i < reviews.Count; i++)
            prompt += $"Review {i}:\n{reviews[i].Review}\n\n";

        prompt += "Based on these reviews, evaluate the employee on a scale from 1 to 5 for the following criteria:\n";
        prompt += "1. Professionalism\n2. Teamwork\n3. Communication\n4. Initiative\n5. Overall Performance\n";
        prompt += "Add short (5 sentences) explanation for each score you assigned.";

        return prompt;
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
