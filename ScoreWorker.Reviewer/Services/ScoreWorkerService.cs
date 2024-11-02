using AutoMapper;
using Refit;
using SampleSolution.Services.Interfaces;
using ScoreWorker.Models.DTO;
using ScoreWorker.RefitApi;
using ScoreWorkerDB.Interfaces;
using System.Text;
using System.Text.Json;

namespace SampleSolution.Services;

public class ScoreWorkerService : IScoreWorkerService
{
    private const string file = "review_dataset.json";
    private const string filePrompt = "prompt.txt";

    private readonly IDataProvider _provider;
    private readonly IMapper _mapper;

    public ScoreWorkerService(
        IDataProvider provider,
        IMapper mapper)
    {
        _provider = provider;
        _mapper = mapper;
    }

    public async Task<string> GetResponse(int id)
    {
        var allReviews = await LoadReviews();

        var reviews = allReviews.Where(r => r.IDUnderReview == id).ToList();

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

        for (int i = 0; i < reviews.Count; i++)
            builder.AppendLine($"Review {i}:\n{reviews[i].Review}");

        string jsonString = await File.ReadAllTextAsync(filePrompt);

        return string.Format(jsonString, builder.ToString());
    }

    private async Task<string> EvaluateReviewsWithLLM(string prompt)
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
