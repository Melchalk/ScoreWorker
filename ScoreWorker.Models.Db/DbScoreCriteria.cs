using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ScoreWorker.Models.Enum;

namespace ScoreWorker.Models.Db;

public class DbScoreCriteria
{
    public const string TableName = "ScoreCriteria";

    public Guid Id { get; set; }
    public int IDUnderReview { get; set; }
    public ScoreCriteriaType Type { get; set; }
    public int Score { get; set; }
    public required string Description { get; set; }

    public DbSummary? Summary { get; set; }
}

public class DbCriteriaConfiguration : IEntityTypeConfiguration<DbScoreCriteria>
{
    public void Configure(EntityTypeBuilder<DbScoreCriteria> builder)
    {
        builder.HasKey(o => o.Id);

        builder
            .HasOne(sc => sc.Summary)
            .WithMany(s => s.ScoreCriteria)
            .HasForeignKey(sc => sc.IDUnderReview)
            .HasPrincipalKey(s => s.IDUnderReview);
    }
}