using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ScoreWorker.Models.Db;
using ScoreWorker.Models.Enum;

namespace ScoreWorker.DB.Migrations;

[DbContext(typeof(ScoreWorkerDbContext))]
[Migration("021120241438_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: DbReview.TableName,
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                IDReviewer = table.Column<int>(nullable: true),
                IDUnderReview = table.Column<int>(nullable: false),
                Review = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Reviews", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: DbSummary.TableName,
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                IDUnderReview = table.Column<int>(nullable: false),
                AllReviewCount = table.Column<int>(nullable: false),
                PositiveReviewCount = table.Column<int>(nullable: false),
                Summary = table.Column<string>(nullable: false),
                SelfSummary = table.Column<string>(nullable: false),
                SummaryByOwnReviews = table.Column<string>(nullable: true),
                PositiveQuality = table.Column<string>(nullable: false),
                NegativeQuality = table.Column<string>(nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Summaries", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: DbScoreCriteria.TableName,
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                IDUnderReview = table.Column<int>(nullable: false),
                Type = table.Column<ScoreCriteriaType>(nullable: false),
                Score = table.Column<int>(nullable: false),
                Description = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ScoreCriteria", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: DbCountingReviews.TableName,
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                IDUnderReview = table.Column<int>(nullable: false),
                Type = table.Column<ReviewType>(nullable: false),
                Count = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CountingReviews", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: DbReview.TableName);
        migrationBuilder.DropTable(name: DbSummary.TableName);
        migrationBuilder.DropTable(name: DbScoreCriteria.TableName);
        migrationBuilder.DropTable(name: DbCountingReviews.TableName);
    }
}
