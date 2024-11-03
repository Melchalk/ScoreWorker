using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScoreWorker.DB.Interfaces;
using ScoreWorker.Domain.Interfaces;
using ScoreWorker.Models.DTO;
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

    public async Task<GetSummaryResponse> GetWorkersScore(int id, CancellationToken cancellationToken)
    {
        string samplePrompt = (await File.ReadAllTextAsync("text.txt", cancellationToken))
            .Replace("\\n", "\n");

        var mainSummary = _promptParser.ParseMainSummary(samplePrompt);

        /*var dbSummary = await _provider.Summaries
            .AsNoTracking()
            .Include(s => s.Reviews)
            .Include(s => s.ScoreCriteria)
            .Include(s => s.CountingReviews)
            .FirstOrDefaultAsync(s => s.IDUnderReview == id)
        ?? throw new BadRequestException($"Review with IDUnderReview = '{id}' was not found.");
        */
        return mainSummary;
    }

    public async Task<GetSummaryResponse> GenerateWorkersScore(int id, CancellationToken cancellationToken)
    {
        var mainSummary = await GetMainSummary(id, cancellationToken);

        var response = _promptParser.ParseMainSummary(mainSummary);

        response.SelfSummary = await GetSelfSummary(id, cancellationToken);

        return response;
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

        return await _promptHandler.GetSummary(reviews, cancellationToken);
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

        return await _promptHandler.GetSummary(reviews, cancellationToken, isMain: false);
    }
}
