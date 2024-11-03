using Microsoft.AspNetCore.Mvc;
using ScoreWorker.Domain.Interfaces;

namespace ScoreWorker.Controllers;

[Route("api/test")]
[ApiController]
public class TestController(
    [FromServices] ITestSolution sampleSolution,
    [FromServices] IScoreWorkerService service)
    : ControllerBase
{
    [HttpGet("generate")]
    public async Task<string> GetTestScore(CancellationToken token)
    {
        return await sampleSolution.GetResponse();
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
}
