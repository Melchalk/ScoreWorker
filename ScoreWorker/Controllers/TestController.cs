using Microsoft.AspNetCore.Mvc;
using SampleSolution.Services.Interfaces;

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
}
