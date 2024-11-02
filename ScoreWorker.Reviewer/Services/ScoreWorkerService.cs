using AutoMapper;
using Refit;
using ScoreWorker.DB.Interfaces;
using ScoreWorker.Domain.Services.Interfaces;
using ScoreWorker.Models.DTO;
using ScoreWorker.RefitApi;
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

    public async Task<string> GetMainSummary(int id, CancellationToken cancellationToken)
    {
        var allReviews = await LoadReviews(cancellationToken);

        var reviews = allReviews!
            .Where(r => r.IDUnderReview == id && r.IDReviewer != id)
            .ToList();
        var prompt = await PreparePrompt(reviews, cancellationToken);

        return await EvaluateReviewsWithLLM(prompt, cancellationToken);
    }

    private async Task<List<ReviewInfo>?> LoadReviews(CancellationToken cancellationToken)
    {
        string jsonString = await File.ReadAllTextAsync(fileDb, cancellationToken);

        return JsonSerializer.Deserialize<List<ReviewInfo>>(jsonString);
    }

    private async Task<string> PreparePrompt(List<ReviewInfo> reviews, CancellationToken cancellationToken)
    {
        StringBuilder builder = new();

        for (int i = 1; i <= reviews.Count; i++)
            builder.AppendLine($"Review {i}:\n{reviews[i-1].Review}");

        string jsonString = await File.ReadAllTextAsync(mainPrompt, cancellationToken);

        return string.Format(jsonString, builder.ToString());
    }

    private async Task<string> EvaluateReviewsWithLLM(string prompt, CancellationToken cancellationToken)
    {
        var apiService = RestService.For<IVkControllerApi>(IVkControllerApi.VkScoreWorkerApi);

        var request = new GenerateScoreRequest()
        {
            Prompt = prompt,
            ApplyChatTemplate = true,
            SystemPrompt = "You are a helpful assistant of an HR speacialist that rates employees of the company they work at.",
            N = 1,
            Temperature = 0.3
        };

        return await apiService.GenerateScore(request);
    }

    #endregion

    #region Self

    public async Task<string> GetSelfSummary(int id, CancellationToken cancellationToken)
    {
        var allReviews = await LoadReviews(cancellationToken);

        var reviews = allReviews!
            .Where(r => r.IDUnderReview == id && r.IDReviewer != id)
            .ToList();

        var prompt = await PreparePrompt(reviews, cancellationToken);

        return await EvaluateReviewsWithLLM(prompt, cancellationToken);
    }

    #endregion
}
