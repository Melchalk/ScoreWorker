using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreWorker.Models.Enum;

namespace ScoreWorker.Models.Db;

public class DbCountingReviews
{
    public const string TableName = "CountingReviews";

    public Guid Id { get; set; }
    public int IDUnderReview { get; set; }
    public ReviewType Type { get; set; }
    public int Count { get; set; }

    public DbSummary? Summary { get; set; }
}

public class DbCountingReviewsConfiguration : IEntityTypeConfiguration<DbCountingReviews>
{
    public void Configure(EntityTypeBuilder<DbCountingReviews> builder)
    {
        builder.HasKey(o => o.Id);

        builder
            .HasOne(cr => cr.Summary)
            .WithMany(s => s.CountingReviews)
            .HasForeignKey(cr => cr.IDUnderReview)
            .HasPrincipalKey(s => s.IDUnderReview);
    }
}