namespace ScoreWorker.Domain.Services.Interfaces;

public interface IScoreWorkerService
{
    public Task<string> GetMainSummary(int id);
    public Task<string> GetSelfSummary(int id);
}
