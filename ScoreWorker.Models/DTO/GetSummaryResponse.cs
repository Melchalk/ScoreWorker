namespace ScoreWorker.Models.DTO;

public class GetSummaryResponse
{
    public int IDUnderReview { get; set; }
    public required string Summary { get; set; }
    public double Score { get; set; }

    public required List<ScoreCriteriaInfo> ScoreCriteria { get; set; }

    public required string SelfSummary { get; set; }
    public required string SummaryByOwnReviews { get; set; }

    public required string PositiveQuality { get; set; }
    public required string NegativeQuality { get; set; }
    public int PositiveReviewCount { get; set; }
    public int AllReviewCount { get; set; }
}