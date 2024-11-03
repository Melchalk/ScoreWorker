using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ScoreWorker.Models.Db;

public class DbReview
{
    public const string TableName = "Reviews";

    public Guid Id { get; set; }
    public int? IDReviewer { get; set; }
    public int IDUnderReview { get; set; }
    public required string Review { get; set; }

    public DbSummary? Summary { get; set; }
}

public class DbReviewConfiguration : IEntityTypeConfiguration<DbReview>
{
    public void Configure(EntityTypeBuilder<DbReview> builder)
    {
        builder.HasKey(o => o.Id);


        builder
            .HasOne(r => r.Summary)
            .WithMany(s => s.Reviews)
            .HasForeignKey(r => r.IDUnderReview)
            .HasPrincipalKey(s => s.IDUnderReview);
    }
}