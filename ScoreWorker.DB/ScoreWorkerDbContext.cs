using Microsoft.EntityFrameworkCore;
using ScoreWorker.DB.Interfaces;
using ScoreWorker.Models.Db;
using System.Reflection;

namespace ScoreWorker.DB;

public class ScoreWorkerDbContext : DbContext, IDataProvider
{
    public DbSet<DbReview> Reviews { get; set; }
    public DbSet<DbScoreCriteria> ScoreCriteria { get; set; }
    public DbSet<DbSummary> Summaries { get; set; }
    public DbSet<DbCountingReviews> CountingReviews { get; set; }

    public ScoreWorkerDbContext(DbContextOptions<ScoreWorkerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("ScoreWorker.Models.Db"));
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
