namespace ScoreWorker.Domain.Interfaces;

public interface ITestSolution
{
    public Task<string> GetResponse();
    public Task UpdateDatabase(CancellationToken cancellationToken);
}
