namespace ScoreWorker.Domain.Interfaces;

public interface IScoreWorkerService
{
    public Task<string> GetMainSummary(int id, CancellationToken cancellationToken);
    public Task<string> GetSelfSummary(int id, CancellationToken cancellationToken);
}
