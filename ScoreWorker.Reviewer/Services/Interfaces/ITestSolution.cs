﻿namespace ScoreWorker.Domain.Services.Interfaces;

public interface ITestSolution
{
    public Task<string> GetResponse();
}