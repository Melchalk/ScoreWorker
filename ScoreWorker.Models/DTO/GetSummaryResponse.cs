using ScoreWorker.Models.Enum;

namespace ScoreWorker.Models.DTO;

public class GetSummaryResponse
{
    public required string Summary { get; set; }
    public double Score { get; set; }

    public required Dictionary<ScoreCriteriaType, ScoreCriteriaInfo> ScoreCriteria { get; set; }

    public required string SelfSummary { get; set; }

    public required string PositiveQuality { get; set; }
    public required string NegativeQuality { get; set; }
    public int PositiveReviewCount { get; set; }
    public int NegativeReviewCount { get; set; }
}
