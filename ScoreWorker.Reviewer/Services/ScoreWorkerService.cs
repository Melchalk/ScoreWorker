using AutoMapper;
using Refit;
using ScoreWorker.Domain.Services.Interfaces;
using ScoreWorker.Models.DTO;
using ScoreWorker.RefitApi;
using ScoreWorkerDB.Interfaces;
using System.Text;
using System.Text.Json;

namespace ScoreWorker.Domain.Services;

public class ScoreWorkerService : IScoreWorkerService
{
    private const string fileDb = "review_dataset.json";
    private const string mainPrompt = "prompt.txt";

    private readonly IDataProvider _provider;
    private readonly IMapper _mapper;

    public ScoreWorkerService(
        IDataProvider provider,
        IMapper mapper)
    {
        _provider = provider;
        _mapper = mapper;
    }

    #region Main

    public async Task<string> GetMainSummary(int id)
    {
        var allReviews = await LoadReviews();

        var reviews = allReviews!
            .Where(r => r.IDUnderReview == id && r.IDReviewer != id)
            .ToList();
        var prompt = await PreparePrompt(reviews);

        return await EvaluateReviewsWithLLM(prompt);
    }

    private async Task<List<ReviewInfo>?> LoadReviews()
    {
        string jsonString = await File.ReadAllTextAsync(fileDb);

        return JsonSerializer.Deserialize<List<ReviewInfo>>(jsonString);
    }

    private async Task<string> PreparePrompt(List<ReviewInfo> reviews)
    {
        StringBuilder builder = new();

        for (int i = 1; i <= reviews.Count; i++)
            builder.AppendLine($"Review {i}:\n{reviews[i].Review}");

        string jsonString = await File.ReadAllTextAsync(mainPrompt);

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
            N = 1,
            Temperature = 0.7
        };

        return await apiService.GenerateScore(request);
    }

    #endregion

    #region Self

    public async Task<string> GetSelfSummary(int id)
    {
        var allReviews = await LoadReviews();

        var reviews = allReviews!
            .Where(r => r.IDUnderReview == id && r.IDReviewer != id)
            .ToList();

        var prompt = await PreparePrompt(reviews);

        return await EvaluateReviewsWithLLM(prompt);
    }

    #endregion
}
