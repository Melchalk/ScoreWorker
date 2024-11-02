using AutoMapper;
using Refit;
using ScoreWorker.DB.Interfaces;
using ScoreWorker.Domain.Services.Interfaces;
using ScoreWorker.Models.Db;
using ScoreWorker.Models.DTO;
using ScoreWorker.RefitApi;
using System.Data.Entity;
using System.Text;
using System.Text.Json;

namespace ScoreWorker.Domain.Services;

public class TestSolution : ITestSolution
{
    private const string fileTest = "sample_reviews.json";
    private const string fileDb = "review_dataset.json";
    private const string filePrompt = "prompt.txt";

    private readonly IDataProvider _provider;
    private readonly IMapper _mapper;

    public TestSolution(
        IDataProvider provider,
        IMapper mapper)
    {
        _provider = provider;
        _mapper = mapper;
    }

    public async Task<string> GetResponse()
    {
        var reviews = await LoadReviews();

        var prompt = await PreparePrompt(reviews);

        return await EvaluateReviewsWithLLM(prompt);
    }

    public async Task UpdateDatabase(CancellationToken cancellationToken)
    {
        string jsonString = await File.ReadAllTextAsync(fileDb, cancellationToken);

        var reviews = JsonSerializer.Deserialize<List<ReviewInfo>>(jsonString);

        var dbReviews = reviews.Select(r => _mapper.Map<DbReview>(r)).ToList();

        await _provider.Reviews.AddRangeAsync(dbReviews, cancellationToken);

        await _provider.SaveAsync(cancellationToken);
    }

    #region Private

    private async Task<List<ReviewInfo>?> LoadReviews()
    {
        string jsonString = await File.ReadAllTextAsync(fileTest);

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

        return await apiService.GenerateScore(request);
    }

    #endregion
}
