namespace ScoreWorker.Models.Db;

public class DbSummary
{
    public Guid Id { get; set; }
    public int IDUnderReview { get; set; }
    public int CountReviewTo { get; set; }
    public int CountReviewFrom { get; set; }
    public double UtilityCoefficient { get; set; }
    public double SocialRating { get; set; }
}
