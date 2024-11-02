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

    public DbReview? Review { get; set; }
}

public class DbSummaryConfiguration : IEntityTypeConfiguration<DbSummary>
{
    public void Configure(EntityTypeBuilder<DbSummary> builder)
    {
        builder.HasKey(o => o.Id);
        /*
        builder
            .HasOne(sc => sc.Review)
            .WithMany(r => r.Summaries)
            .HasForeignKey(sc => sc.IDUnderReview)
            .HasPrincipalKey(r => r.IDUnderReview);*/
    }
}