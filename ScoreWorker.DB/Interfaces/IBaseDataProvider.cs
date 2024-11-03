namespace ScoreWorker.DB.Interfaces;

public interface IBaseDataProvider
{
    Task SaveAsync(CancellationToken token);

    void Save();
}
