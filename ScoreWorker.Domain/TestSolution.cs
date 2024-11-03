using AutoMapper;
using ScoreWorker.DB.Interfaces;
using ScoreWorker.Domain.Interfaces;
using ScoreWorker.Models.Db;
using ScoreWorker.Models.DTO;
using System.Text.Json;

namespace ScoreWorker.Domain;

public class TestSolution : ITestSolution
{
    private const string fileDb = "review_dataset.json";

    private readonly IDataProvider _provider;
    private readonly IMapper _mapper;

    public TestSolution(
        IDataProvider provider,
        IMapper mapper)
    {
        _provider = provider;
        _mapper = mapper;
    }

    public async Task UpdateDatabase(CancellationToken cancellationToken)
    {
        string jsonString = await File.ReadAllTextAsync(fileDb, cancellationToken);

        var reviews = JsonSerializer.Deserialize<List<ReviewInfo>>(jsonString);

        var dbReviews = reviews!.Select(r => _mapper.Map<DbReview>(r)).ToList();

        await _provider.Reviews.AddRangeAsync(dbReviews, cancellationToken);

        await _provider.SaveAsync(cancellationToken);
    }
}
