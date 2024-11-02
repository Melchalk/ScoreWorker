using ScoreWorker.Models.Enum;

namespace ScoreWorker.Models.Db;

public class DbCountingReviews
{
    public Guid Id { get; set; }
    public int IDUnderReview { get; set; }
    public ReviewType Type { get; set; }
    public int Count { get; set; }
}
