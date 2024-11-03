using Hangfire;
using Microsoft.AspNetCore.Mvc;
using ScoreWorker.Domain.Interfaces;
using ScoreWorker.Models.DTO;

namespace ScoreWorker.Controllers;

[Route("api")]
[ApiController]
public class ScoreWorkerController([FromServices] IScoreWorkerService service) : ControllerBase
{
    [HttpGet("get/summary")]
    public async Task<GetSummaryResponse> GetAfterSummary([FromQuery] int id, CancellationToken token)
    {
        return await service.GetWorkersScore(id, token);
    }

    [HttpGet("get/opinion")]
    public async Task<string> GetOpinion(
        [FromQuery] int currentId,
        [FromQuery] int researchId,
        CancellationToken token)
    {
        return await service.GetOpinion(currentId, researchId, token);
    }

    [HttpGet("generate")]
    public void GenerateWorkersScore([FromQuery] int id, CancellationToken token)
    {
        var now = DateTime.UtcNow;

        RecurringJob.AddOrUpdate("GenerateWorkersScore", () =>
            service.GenerateWorkersScore(id, token), $"{now.Minute + 1} {now.Hour} * * *");
    }
}
