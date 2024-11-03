using Microsoft.EntityFrameworkCore;
using ScoreWorker.Models.Db;

namespace ScoreWorker.DB.Interfaces;

public interface IDataProvider : IBaseDataProvider
{
    DbSet<DbReview> Reviews { get; set; }
    DbSet<DbSummary> Summaries { get; set; }
    DbSet<DbScoreCriteria> ScoreCriteria { get; set; }
    DbSet<DbCountingReviews> CountingReviews { get; set; }
}