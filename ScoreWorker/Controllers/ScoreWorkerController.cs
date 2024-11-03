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

    [HttpGet("generate")]
    public async Task GenerateWorkersScore([FromQuery] int id, CancellationToken token)
    {
        await service.GenerateWorkersScore(id, token);
    }
}
