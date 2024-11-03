using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ScoreWorker.Models.Db;

public class DbSummary
{
    public const string TableName = "Summaries";

    public Guid Id { get; set; }
    public int IDUnderReview { get; set; }
    public int CountReviewTo { get; set; }
    public int CountReviewFrom { get; set; }
    public double UtilityCoefficient { get; set; }
    public double SocialRating { get; set; }

    public ICollection<DbCountingReviews> CountingReviews { get; set; }
    public ICollection<DbScoreCriteria> ScoreCriteria { get; set; }
    public ICollection<DbReview> Reviews { get; set; }

    public DbSummary()
    {
        CountingReviews = new HashSet<DbCountingReviews>();
        ScoreCriteria = new HashSet<DbScoreCriteria>();
        Reviews = new HashSet<DbReview>();
    }
}

public class DbSummaryConfiguration : IEntityTypeConfiguration<DbSummary>
{
    public void Configure(EntityTypeBuilder<DbSummary> builder)
    {
        builder.HasKey(o => o.Id);
    }
}