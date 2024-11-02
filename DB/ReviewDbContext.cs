using Microsoft.EntityFrameworkCore;
using ScoreWorker.Models.Db;
using ScoreWorkerDB.Interfaces;

namespace ScoreWorkerDB;

public class ReviewDbContext : DbContext, IDataProvider
{
    public DbSet<DbReview> Reviews { get; set; }
    public DbSet<DbCriteria> Criteria { get; set; }
    public DbSet<DbSummary> Summaries { get; set; }
    public DbSet<DbCountingReviews> CountingReviews { get; set; }

    public ReviewDbContext(DbContextOptions<ReviewDbContext> options) : base(options)
    {
    }

    public async Task SaveAsync(CancellationToken token)
    {
        await SaveChangesAsync(token);
    }

    public void Save()
    {
        SaveChanges();
    }
}
