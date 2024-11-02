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

    public DbReview? Review { get; set; }
}

public class DbCountingReviewsConfiguration : IEntityTypeConfiguration<DbCountingReviews>
{
    public void Configure(EntityTypeBuilder<DbCountingReviews> builder)
    {
        builder.HasKey(o => o.Id);
        /*
        builder
            .HasOne(cr => cr.Review)
            .WithMany(r => r.CountingReviews)
            .HasForeignKey(cr => cr.IDUnderReview)
            .HasPrincipalKey(r => r.IDUnderReview);*/
    }
}