using ScoreWorker.Models.DTO;

namespace ScoreWorker.Domain.Interfaces;

/// <summary>
/// Intermediate service for interaction between AI and database
/// </summary>
public interface IScoreWorkerService
{
    public Task<GetSummaryResponse> GetWorkersScore(int id, CancellationToken cancellationToken);
    public Task<GetSummaryResponse> GenerateWorkersScore(int id, CancellationToken cancellationToken);
    public Task<string> GetOpinion(int currentId, int researchId, CancellationToken cancellationToken);
    public Task<string> GetOwnReviewsSummary(int id, CancellationToken cancellationToken);
    public Task<string> GetMainSummary(int id, CancellationToken cancellationToken);
    public Task<string> GetSelfSummary(int id, CancellationToken cancellationToken);
}
