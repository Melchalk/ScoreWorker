using Microsoft.AspNetCore.Mvc;
using ScoreWorker.Domain.Interfaces;
using ScoreWorker.Models.DTO;

namespace ScoreWorker.Controllers;

[Route("api")]
[ApiController]
public class ScoreWorkerController([FromServices] IScoreWorkerService service) : ControllerBase
{
    [HttpGet("generate")]
    public async Task<string> GetTextScore([FromQuery] int id, CancellationToken token)
    {
        return await service.GetMainSummary(id, token);
    }
    
    [HttpGet("generate/self")]
    public async Task<string> GetTextSelfScore([FromQuery] int id, CancellationToken token)
    {
        return await service.GetSelfSummary(id, token);
    }

    [HttpGet("get/summary")]
    public async Task<GetSummaryResponse> GetAfterSummary([FromQuery] int id, CancellationToken token)
    {
        return await service.GetWorkersScore(id, token);
    }
}
