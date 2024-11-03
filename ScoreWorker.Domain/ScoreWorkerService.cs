using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScoreWorker.DB.Interfaces;
using ScoreWorker.Domain.Interfaces;
using ScoreWorker.Models.Db;
using ScoreWorker.Models.DTO;
using ScoreWorker.Models.Enum;
using ScoreWorker.Prompt.Interfaces;
using WebLibrary.Backend.Models.Exceptions;

namespace ScoreWorker.Domain;

public class ScoreWorkerService : IScoreWorkerService
{
    private readonly IDataProvider _provider;
    private readonly IMapper _mapper;
    private readonly IPromptHandler _promptHandler;
    private readonly IPromptParser _promptParser;

    public ScoreWorkerService(
        IDataProvider provider,
        IMapper mapper,
        IPromptParser promptParser,
        IPromptHandler promptHandler)
    {
        _provider = provider;
        _mapper = mapper;
        _promptHandler = promptHandler;
        _promptParser = promptParser;
    }

    /// <summary>
    ///  Method to get summary from database
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public async Task<GetSummaryResponse> GetWorkersScore(int id, CancellationToken cancellationToken)
    {
        var dbSummary = await _provider.Summaries
            .AsNoTracking()
            .Include(s => s.Reviews)
            .Include(s => s.ScoreCriteria)
            .Include(s => s.CountingReviews)
            .FirstOrDefaultAsync(s => s.IDUnderReview == id, cancellationToken)
        ?? throw new BadRequestException($"Review with IDUnderReview = '{id}' was not found.");

        var response = _mapper.Map<GetSummaryResponse>(dbSummary);
        response.Score = response.ScoreCriteria.Select(s => s.Score).Average();

        return response;
    }

    /// <summary>
    /// Method for generating summary and writing to databases
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GetSummaryResponse> GenerateWorkersScore(int id, CancellationToken cancellationToken)
    {
        var mainSummary = (await GetMainSummary(id, cancellationToken))
            .Replace("\\n", "\n");

        var response = _promptParser.ParseMainSummary(mainSummary);

        response.SelfSummary = await GetSelfSummary(id, cancellationToken);
        response.SummaryByOwnReviews = await GetOwnReviewsSummary(id, cancellationToken);

        response.IDUnderReview = id;
        for (int i = 0; i < response.ScoreCriteria.Count; i++)
        {
            response.ScoreCriteria[i].IDUnderReview = id;
        }

        var dbSummary = _mapper.Map<DbSummary>(response);

        var oldSummary = await _provider.Summaries
            .FirstOrDefaultAsync(s => s.IDUnderReview == id, cancellationToken);

        if (oldSummary is not null)
        {
            _provider.Summaries.Remove(oldSummary);

            await _provider.SaveAsync(cancellationToken);
        }

        await _provider.Summaries.AddAsync(dbSummary, cancellationToken);

        await _provider.SaveAsync(cancellationToken);

        return response;
    }

    public async Task<string> GetMainSummary(int id, CancellationToken cancellationToken)
    {
        var dbReviews = _provider.Reviews
            .AsNoTracking()
            .Where(r => r.IDUnderReview == id && r.IDReviewer != id);

        if (!dbReviews.Any())
        {
            throw new BadRequestException($"Reviews with IDUnderReview = '{id}' were not found.");
        }

        var reviews = await _mapper.ProjectTo<ReviewInfo>(dbReviews)
            .ToListAsync(cancellationToken);

        return await _promptHandler.GetSummary(PromptType.Main, reviews, cancellationToken);
    }

    public async Task<string> GetSelfSummary(int id, CancellationToken cancellationToken)
    {
        var dbReviews = _provider.Reviews
            .AsNoTracking()
            .Where(r => r.IDUnderReview == id && r.IDReviewer == id);

        if (!dbReviews.Any())
        {
            throw new BadRequestException($"Reviews with IDUnderReview = '{id}' were not found.");
        }

        var reviews = await _mapper.ProjectTo<ReviewInfo>(dbReviews)
            .ToListAsync(cancellationToken);

        return await _promptHandler.GetSummary(PromptType.Self, reviews, cancellationToken);
    }

    public async Task<string> GetOpinion(
        int currentId, int researchId, CancellationToken cancellationToken)
    {
        var dbReviews = _provider.Reviews
            .AsNoTracking()
            .Where(r => r.IDUnderReview == researchId && r.IDReviewer == currentId);

        if (!dbReviews.Any())
        {
            throw new BadRequestException($"Reviews with IDUnderReview = '{researchId}' were not found.");
        }

        var reviews = await _mapper.ProjectTo<ReviewInfo>(dbReviews)
            .ToListAsync(cancellationToken);

        return await _promptHandler.GetSummary(PromptType.Opinion, reviews, cancellationToken);
    }

    public async Task<string> GetOwnReviewsSummary(int id, CancellationToken cancellationToken)
    {
        var dbReviews = _provider.Reviews
            .AsNoTracking()
            .Where(r => r.IDReviewer == id);

        if (!dbReviews.Any())
        {
            throw new BadRequestException($"Reviews with IDUnderReview = '{id}' were not found.");
        }

        var reviews = await _mapper.ProjectTo<ReviewInfo>(dbReviews)
            .ToListAsync(cancellationToken);

        return await _promptHandler.GetSummary(PromptType.OwnReviews, reviews, cancellationToken);
    }
}
