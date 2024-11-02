using Microsoft.EntityFrameworkCore.Migrations;
using ScoreWorker.Models.Enum;

namespace ScoreWorkerDB.Migrations;

[Migration("021120241438_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Reviews",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                IDReviewer = table.Column<int>(nullable: false),
                IDUnderReview = table.Column<int>(nullable: false),
                Review = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Reviews", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Summaries",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                IDUnderReview = table.Column<int>(nullable: false),
                CountReviewTo = table.Column<int>(nullable: false),
                CountReviewFrom = table.Column<int>(nullable: false),
                UtilityCoefficient = table.Column<double>(nullable: false),
                SocialRating = table.Column<double>(nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Summaries", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Criteria",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Type = table.Column<ScoreCriteriaType>(nullable: false),
                Score = table.Column<int>(nullable: false),
                Description = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Criteria", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CountingReviews",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                IDUnderReview = table.Column<int>(nullable: false),
                Type = table.Column<ReviewType>(nullable: false),
                Count = table.Column<int>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CountingReviews", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Reviews");
        migrationBuilder.DropTable(name: "Summaries");
        migrationBuilder.DropTable(name: "Criteria");
        migrationBuilder.DropTable(name: "CountingReviews");
    }
}
