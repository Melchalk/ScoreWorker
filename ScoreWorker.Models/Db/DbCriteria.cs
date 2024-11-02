using ScoreWorker.Models.Enum;

namespace ScoreWorker.Models.Db;

public class DbCriteria
{
    public Guid Id { get; set; }
    public ScoreCriteriaType Type { get; set; }
    public int Score { get; set; }
    public required string Description { get; set; }
}