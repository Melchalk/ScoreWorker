using Microsoft.AspNetCore.Mvc;
using ScoreWorker.Domain.Interfaces;
using ScoreWorker.Models.DTO;

namespace ScoreWorker.Controllers;

[Route("api/test")]
[ApiController]
public class TestController(
    [FromServices] ITestSolution sampleSolution,
    [FromServices] IScoreWorkerService service)
    : ControllerBase
{
    [HttpGet("generate/get/model")]
    public async Task<GetSummaryResponse> GenerateWorkersScore([FromQuery] int id, CancellationToken token)
    {
        return await service.GenerateWorkersScore(id, token);
    }

    [HttpGet("update/database")]
    public async Task UpdateDatabasByFile(CancellationToken token)
    {
        await sampleSolution.UpdateDatabase(token);
    }

    [HttpGet("generate/byMainFile")]
    public async Task<string> GetTextScore([FromQuery] int id, CancellationToken token)
    {
        return await service.GetMainSummary(id, token);
    }

    [HttpGet("generate/self")]
    public async Task<string> GetTextSelfScore([FromQuery] int id, CancellationToken token)
    {
        return await service.GetSelfSummary(id, token);
    }

    [HttpGet("generate/summary/by/own")]
    public async Task<string> GetOwnReviewsSummary([FromQuery] int id, CancellationToken token)
    {
        return await service.GetOwnReviewsSummary(id, token);
    }
}
