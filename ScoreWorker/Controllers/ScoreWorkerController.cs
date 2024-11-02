using Microsoft.AspNetCore.Mvc;
using ScoreWorker.Domain.Services.Interfaces;

namespace ScoreWorker.Controllers;

[Route("api")]
[ApiController]
public class ScoreWorkerController([FromServices] IScoreWorkerService service) : ControllerBase
{
    [HttpGet("generate")]
    public async Task<string> GetTestScore([FromQuery] int id, CancellationToken token)
    {
        return await service.GetMainSummary(id);
    }
}
