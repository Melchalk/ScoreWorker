using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ScoreWorker.Models.Db;

public class DbReview
{
    public const string TableName = "Reviews";

    [Key]
    public Guid Id { get; set; }
    public int? IDReviewer { get; set; }
    public int IDUnderReview { get; set; }
    public required string Review { get; set; }

    public ICollection<DbCountingReviews> CountingReviews { get; set; }
    public ICollection<DbScoreCriteria> ScoreCriteria { get; set; }
    public ICollection<DbSummary> Summaries { get; set; }

    public DbReview()
    {
        CountingReviews = new HashSet<DbCountingReviews>();
        ScoreCriteria = new HashSet<DbScoreCriteria>();
        Summaries = new HashSet<DbSummary>();
    }
}

public class DbReviewConfiguration : IEntityTypeConfiguration<DbReview>
{
    public void Configure(EntityTypeBuilder<DbReview> builder)
    {
        builder.HasKey(o => o.Id);
    }
}