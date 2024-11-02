using Microsoft.AspNetCore.Mvc;
using ScoreWorker.Domain.Services.Interfaces;

namespace ScoreWorker.Controllers;

[Route("api/test")]
[ApiController]
public class TestController([FromServices] ITestSolution sampleSolution) : ControllerBase
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

}
