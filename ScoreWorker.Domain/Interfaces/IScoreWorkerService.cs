using ScoreWorker.Models.DTO;

namespace ScoreWorker.Domain.Interfaces;

public interface IScoreWorkerService
{
    public Task<GetSummaryResponse> GetWorkersScore(int id, CancellationToken cancellationToken);
    public Task<GetSummaryResponse> GenerateWorkersScore(int id, CancellationToken cancellationToken);
    public Task<string> GetOpinion(int currentId, int researchId, CancellationToken cancellationToken);
    public Task<string> GetMainSummary(int id, CancellationToken cancellationToken);
    public Task<string> GetSelfSummary(int id, CancellationToken cancellationToken);
}
