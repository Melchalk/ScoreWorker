namespace ScoreWorker.Domain.Interfaces;

/// <summary>
/// Service for working with mock data, previously contained test methods
/// </summary>
public interface ITestSolution
{
    public Task UpdateDatabase(CancellationToken cancellationToken);
}
