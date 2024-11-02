using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Refit;
using ScoreWorker.DB.Interfaces;
using ScoreWorker.Domain.Interfaces;
using ScoreWorker.Models.DTO;
using ScoreWorker.RefitApi;
using System.Text;
using WebLibrary.Backend.Models.Exceptions;

namespace ScoreWorker.Domain;

public class ScoreWorkerService : IScoreWorkerService
{
    private const string mainPrompt = "MainPrompt.txt";
    private const string selfPrompt = "SelfPrompt.txt";

    private readonly IDataProvider _provider;
    private readonly IMapper _mapper;

    public ScoreWorkerService(
        IDataProvider provider,
        IMapper mapper)
    {
        _provider = provider;
        _mapper = mapper;
    }

    public async Task<string> GetMainSummary(int id, CancellationToken cancellationToken)
    {
        var dbReviews = _provider.Reviews
            .AsNoTracking()
            .Where(r => r.IDUnderReview == id && r.IDReviewer != id);

        if (!dbReviews.Any())
        {
            throw new BadRequestException($"Reviews with IDUnderReview = '{id}' was not found.");
        }

        var reviews = await _mapper.ProjectTo<ReviewInfo>(dbReviews)
            .ToListAsync(cancellationToken);

        var prompt = await PreparePrompt(mainPrompt, reviews, cancellationToken);

        return await EvaluateReviewsWithLLM(prompt, cancellationToken);
    }

    public async Task<string> GetSelfSummary(int id, CancellationToken cancellationToken)
    {
        var dbReviews = _provider.Reviews
            .AsNoTracking()
            .Where(r => r.IDUnderReview == id && r.IDReviewer == id);

        if (!dbReviews.Any())
        {
            throw new BadRequestException($"Reviews with IDUnderReview = '{id}' was not found.");
        }

        var reviews = await _mapper.ProjectTo<ReviewInfo>(dbReviews)
            .ToListAsync(cancellationToken);

        var prompt = await PreparePrompt(selfPrompt, reviews, cancellationToken);

        return await EvaluateReviewsWithLLM(prompt, cancellationToken);
    }

    #region Private

    private async Task<string> PreparePrompt(
        string filePrompt, List<ReviewInfo> reviews, CancellationToken cancellationToken)
    {
        StringBuilder builder = new();

        for (int i = 1; i <= reviews.Count; i++)
            builder.AppendLine($"Review {i}:\n{reviews[i - 1].Review}");

        string samplePrompt = await File.ReadAllTextAsync(filePrompt, cancellationToken);

        return string.Format(samplePrompt, builder.ToString());
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
}
