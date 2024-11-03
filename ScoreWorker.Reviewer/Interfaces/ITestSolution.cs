namespace ScoreWorker.Domain.Interfaces;

public interface ITestSolution
{
    public Task UpdateDatabase(CancellationToken cancellationToken);
}
