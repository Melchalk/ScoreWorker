﻿using Microsoft.EntityFrameworkCore;
using ScoreWorker.Models.Db;

namespace ScoreWorkerDB.Interfaces;

public interface IDataProvider : IBaseDataProvider
{
    DbSet<DbReview> Reviews { get; set; }
    DbSet<DbSummary> Summaries { get; set; }
    DbSet<DbCriteria> Criteria { get; set; }
    DbSet<DbCountingReviews> CountingReviews { get; set; }
}