using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ScoreWorker.Models.Db;

public class DbReview
{
    public const string TableName = "Reviews";

    public Guid Id { get; set; }
    public int? IDReviewer { get; set; }
    public int IDUnderReview { get; set; }
    public required string Review { get; set; }

    public ICollection<DbCountingReviews> CountingReviews { get; set; } = new HashSet<DbCountingReviews>();
    public ICollection<DbScoreCriteria> ScoreCriteria { get; set; } = new HashSet<DbScoreCriteria>();
    public ICollection<DbSummary> Summaries { get; set; } = new HashSet<DbSummary>();
}

public class DbReviewConfiguration : IEntityTypeConfiguration<DbReview>
{
    public void Configure(EntityTypeBuilder<DbReview> builder)
    {
        builder.HasKey(o => o.Id);
    }
}