namespace SampleSolution.Services.Interfaces;

public interface IScoreWorkerService
{
    public Task<string> GetResponse(int id);
}
