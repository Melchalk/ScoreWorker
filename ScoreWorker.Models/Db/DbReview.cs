namespace ScoreWorker.Models.Db;

public class DbReview
{
    public Guid Id { get; set; }
    public int? IDReviewer { get; set; }
    public int IDUnderReview { get; set; }
    public required string Review { get; set; }
}
